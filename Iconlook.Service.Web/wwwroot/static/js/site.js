$(document).ready(function() {
    let subscriptions = [];
    const subject = new rxjs.Subject();
    $("[data-toggle=tooltip]").tooltip({ delay: { show: 0 } });
    const source = new EventSource("/sse/stream?channel=iconlook");
    let leader_block_mcount = parseInt($(".leader-block-mcount").text());
    source.addEventListener("error", function(e) { console.log("ERROR", e); }, false);
    $(source).handleServerEvents({
        success: function(selector, json, message) {
            if (!selector.startsWith("cmd.on")) {
                subject.next({ name: message.cmd, body: json });
            }
        },
        handlers: {
            onReconnect: function() {
                console.log("[ICONLOOK] Channel re-connected.");
            },
            onConnect: function() {
                subscriptions.forEach(subscription => {
                    subscription.unsubscribe();
                    subscription = null;
                });
                subscriptions = [];
                console.log("[ICONLOOK] Channel connected.");
                subscriptions.push(subject.subscribe(() => {
                        if (leader_block_mcount <= 10) {
                            let leader_block_elapse = "";
                            let leader_block_remain = "";
                            for (let i = 0; i < leader_block_mcount; i++) {
                                leader_block_elapse += "❚";
                            }
                            for (let i = leader_block_mcount; i < 10; i++) {
                                leader_block_remain += "❚";
                            }
                            $(".leader-block-mcount").text(leader_block_mcount);
                            $(".leader-block-elapse").text(leader_block_elapse);
                            $(".leader-block-remain").text(leader_block_remain);
                        }
                    }));
                // ******************************************
                // *          ChainUpdatedSignal            *
                // ******************************************
                subscriptions.push(subject
                    .pipe(rxjs.operators.filter(x => x.name === "ChainUpdatedSignal"))
                    .pipe(rxjs.operators.throttleTime(1000)).subscribe(signal => {
                        $(".ChainResponse_IcxPrice").money(signal.body.chain.icxPrice, 4);
                        $(".ChainResponse_MarketCap").integer(signal.body.chain.marketCap);
                        $(".ChainResponse_IcxSupply").integer(signal.body.chain.icxSupply);
                        $(".ChainResponse_TotalStaked").integer(signal.body.chain.totalStaked);
                        $(".ChainResponse_TotalUnstaking").integer(signal.body.chain.totalUnstaking);
                        $(".ChainResponse_IcxCirculation").integer(signal.body.chain.icxCirculation);
                        $(".ChainResponse_RRepPercentage").percent(signal.body.chain.rRepPercentage);
                        $(".ChainResponse_PublicTreasury").integer(signal.body.chain.publicTreasury);
                        $(".ChainResponse_TotalDelegated").integer(signal.body.chain.totalDelegated);
                        $(".ChainResponse_NextTermCountdown").text(signal.body.chain.nextTermCountdown);
                        $(".ChainResponse_TransactionCount").integer(signal.body.chain.transactionCount);
                        $(".ChainResponse_StakedPercentage").percent(signal.body.chain.stakedPercentage);
                        $(".ChainResponse_DelegatedPercentage").percent(signal.body.chain.delegatedPercentage);
                        $(".ChainResponse_StakingAddressCount").integer(signal.body.chain.stakingAddressCount);
                        $(".ChainResponse_NextTermBlockHeight").integer(signal.body.chain.nextTermBlockHeight);
                    }));
                // ******************************************
                // *          PeersUpdatedSignal            *
                // ******************************************
                subscriptions.push(subject
                    .pipe(rxjs.operators.filter(x => x.name === "PeersUpdatedSignal"))
                    .pipe(rxjs.operators.throttleTime(1000)).subscribe(signal => {
                        if (signal.body.busy != null) {
                            signal.body.busy.forEach(function(item, index) {
                                // **************************
                                // *  Update Leader State   *
                                // **************************
                                if (index === 0 && $(".leader-name").text() !== item.name) {
                                    leader_block_mcount = 0;
                                    $(".leader-tile").hide();
                                    $(".leader-tile").fadeIn(500);
                                    $(".leader-name").text(item.name);
                                    $(".leader-block-mcount").text("0");
                                    $(".leader-block-elapse").text(null);
                                    $(".leader-logo").attr("src", item.logoUrl);
                                    $(".leader-ranking").text(`#${item.ranking}`);
                                    $(".leader-block-remain").text("❚❚❚❚❚❚❚❚❚❚");
                                }
                                // ***************************
                                // * Update Production State *
                                // ***************************
                                const id = item.peerId.toString();
                                const peer_name = $(`.peer-name-${id}`);
                                const peer_state = $(`.peer-state-${id}`);
                                const peer_state_span = $(`.peer-state-${id} span`);
                                const peer_produced_blocks = $(`.peer-produced-blocks-${id}`);

                                if (peer_state_span.text() === "IDLE") {
                                    peer_state_span.text("BUSY");
                                    peer_name.addClass("font-weight-bold");
                                    peer_state.addClass("peer-state-busy");
                                    peer_state.closest("tr").hide().fadeIn(500);
                                }

                                $(".peer-state span").not(peer_state_span).text("IDLE");
                                $(".peer-name").not(peer_name).removeClass("font-weight-bold");
                                $(".peer-state").not(peer_state).removeClass("peer-state-busy");

                                peer_produced_blocks.attr("number", parseInt(peer_produced_blocks.attr("number")) + 1);
                                peer_produced_blocks.text(
                                    parseInt(peer_produced_blocks.attr("number")).toLocaleString());
                            });
                        }
                    }));
                // ******************************************
                // *          BlockUpdatedSignal           *
                // ******************************************
                subscriptions.push(subject
                    .pipe(rxjs.operators.filter(x => x.name === "BlockUpdatedSignal"))
                    .pipe(rxjs.operators.throttleTime(1000)).subscribe(signal => {
                        $(".BlockResponse_Height").integer(signal.body.block.height);
                        leader_block_mcount += 1;
                        if ($("#block_grid").length) {
                            if ($("#block_grid .e-content tr").length > 1) {
                                if ($("#block_grid .e-content tr").length >= 22) {
                                    $("#block_grid .e-content tr:last").remove();
                                }
                                const block_row = $("#block_grid .e-content tr:first").clone();
                                block_row.find(".BlockResponse_TotalAmount")
                                    .decimal(signal.body.block.totalAmount, 4, true);
                                block_row.find(".BlockResponse_BlockHeight")
                                    .text(signal.body.block.height.toLocaleString());
                                block_row.find(".BlockResponse_BlockHash")
                                    .text(`${signal.body.block.hash.substring(0, 16)}..`);
                                block_row.find(".BlockResponse_TransactionCount")
                                    .integer(signal.body.block.transactionCount, "0000");
                                block_row.hide().css("opacity", 0).css("background-color", "#ffffe1");
                                block_row.prependTo($("#block_grid .e-content tbody:first"));
                                block_row.slideDown(150).animate({ opacity: 1 }, 450)
                                    .animate({ queue: true, 'background-color': "#ffffff" }, 300);
                            }
                            $("#block_grid .e-content tr").toArray().forEach(function(item, index) {
                                $(item).attr("aria-rowindex", index);
                                $(item).attr("data-uid", `grid-row${index}`);
                            });
                        }
                        if ($("#transaction_grid").length) {
                            signal.body.transactions.forEach(function(item, index) {
                                if ($("#transaction_grid .e-content tr").length > 1) {
                                    if ($("#transaction_grid .e-content tr").length >= 22) {
                                        $("#transaction_grid .e-content tr:last").remove();
                                    }
                                    const trans_row = $("#transaction_grid .e-content tr:first").clone();
                                    trans_row.attr("aria-rowindex", index).attr("data-uid", `grid-row${index}`);
                                    trans_row.find(".TransactionResponse_Amount").decimal(item.amount, 4, true);
                                    trans_row.find(".TransactionResponse_To").text(`${item.to.substring(0, 10)}..`);
                                    trans_row.find(".TransactionResponse_From").text(`${item.from.substring(0, 10)}..`);
                                    trans_row.find(".TransactionResponse_Hash").text(`${item.hash.substring(0, 16)}..`);
                                    trans_row.hide().css("opacity", 0).css("background-color", "#fffff0");
                                    trans_row.prependTo($("#transaction_grid .e-content tbody:first"));
                                    trans_row.slideDown(150).animate({ opacity: 1 }, 450)
                                        .animate({ queue: true, 'background-color': "#ffffff" }, 300);
                                }
                            });
                            $("#transactions_grid .e-content tr").toArray().forEach(function(item, index) {
                                $(item).attr("aria-rowindex", index);
                                $(item).attr("data-uid", `grid-row${index}`);
                            });
                        }
                    }));
            }
        }
    });
});