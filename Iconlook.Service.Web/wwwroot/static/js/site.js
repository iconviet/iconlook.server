jQuery.fn.extend({
    integer: function(number, padding) {
        return this.each(function() {
            if ($(this).attr('number') != null) {
                $(this).prop('number', $(this).attr('number'));
                $(this).removeAttr('number');
            }
            if (padding != null) {
                $(this).animateNumber({
                    number: number,
                    numberStep: function(now, tween) {
                        var text = padding + Math.round(now).toString();
                        text = text.substring(Math.round(now).toString().length);
                        $(tween.elem).prop('number', Math.round(now)).text(text);
                    }
                });
            } else {
                $(this).animateNumber({
                    number: number,
                    numberStep: $.animateNumber.numberStepFactories.separator(',')
                });
            }
        });
    },
    percent: function(number) {
        return this.each(function() {
            var factor = Math.pow(10, 4);
            if ($(this).attr('number') != null) {
                $(this).prop('number', $(this).attr('number'));
                $(this).removeAttr('number');
            }
            $(this).animateNumber({
                number: number * factor,
                numberStep: function(now, tween) {
                    var text = (Math.floor(now) / factor * 100).toFixed(2);
                    $(tween.elem).prop('number', Math.round(now)).text(text + '%');
                }
            });
        });
    },
    decimal: function(number, fraction, split = false) {
        return this.each(function() {
            var factor = Math.pow(10, fraction);
            if ($(this).attr('number') != null) {
                $(this).prop('number', $(this).attr('number'));
                $(this).removeAttr('number');
            }
            $(this).animateNumber({
                number: number * factor,
                numberStep: function(now, tween) {
                    var text = (Math.floor(now) / factor).toFixed(fraction);
                    if (split === false) {
                        $(tween.elem).prop('number', Math.round(now)).text(text);
                    } else {
                        var part = text.split('.');
                        if (part.length === 2) {
                            var zero = "number-part";
                            if (parseInt(part[0]) === 0 && parseInt(part[1]) === 0) {
                                zero = "fraction-part";
                            }
                            $(tween.elem).prop('number', Math.round(now)).html(
                                '<span class="' + zero + '">' + part[0] + '</span>' +
                                '<span class="fraction-part">.' + part[1] + '</span>');
                        }
                    }
                }
            });
        });
    },
});
$(document).ready(function() {
    const source = new EventSource('/sse/stream?channel=iconlook');
    var leader_block_mcount = parseInt($('.leader-block-mcount').text());
    source.addEventListener('error', function(e) { console.log("ERROR", e); }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function() {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                // *****************************
                // * Handle ChainUpdatedSignal *
                // *****************************
                if (message.cmd === 'ChainUpdatedSignal') {
                    $('.ChainResponse_MarketCap').integer(json.chain.marketCap);
                    $('.ChainResponse_IcxSupply').integer(json.chain.icxSupply);
                    $('.ChainResponse_IcxPrice').decimal(json.chain.icxPrice, 4);
                    $('.ChainResponse_BlockHeight').integer(json.chain.blockHeight);
                    $('.ChainResponse_TotalStaked').integer(json.chain.totalStaked);
                    $('.ChainResponse_TotalUnstaking').integer(json.chain.totalUnstaking);
                    $('.ChainResponse_IcxCirculation').integer(json.chain.icxCirculation);
                    $('.ChainResponse_PublicTreasury').integer(json.chain.publicTreasury);
                    $('.ChainResponse_TotalDelegated').integer(json.chain.totalDelegated);
                    $('.ChainResponse_TransactionCount').integer(json.chain.transactionCount);
                    $('.ChainResponse_StakedPercentage').percent(json.chain.stakedPercentage);
                    $('.ChainResponse_DelegatedPercentage').percent(json.chain.delegatedPercentage);
                    $('.ChainResponse_StakingAddressCount').integer(json.chain.stakingAddressCount);
                }
                // *****************************
                // * Handle PeersUpdatedSignal *
                // *****************************
                if (message.cmd === 'PeersUpdatedSignal') {
                    if (json.busy != null) {
                        json.busy.forEach(function(item, index) {
                            // ***********************
                            // * Update Leader State *
                            // ***********************
                            if (index === 0 && $('.leader-name').text() !== item.name) {
                                $('.leader').hide().fadeIn(500);
                                $('.leader-name').text(item.name);
                                $('.leader-logo').attr('src', item.logoUrl);
                                $('.leader-ranking').text('#' + item.ranking);
                                $('.leader-block-mcount').text(leader_block_mcount);
                                if (item.madeBlockCount < 10) {
                                    leader_block_mcount = item.madeBlockCount;
                                } else {
                                    leader_block_mcount = 0;
                                    $('.leader-block-mcount').text('0');
                                    $('.leader-block-elapse').text(' ');
                                    $('.leader-block-remain').text('❚❚❚❚❚❚❚❚❚❚');
                                }
                            }
                            // ***************************
                            // * Update Production State *
                            // ***************************
                            const id = item.peerId.toString();
                            const peer_name = $('.peer-name-' + id);
                            const peer_state = $('.peer-state-' + id);
                            const peer_state_span = $('.peer-state-' + id + ' span');
                            const peer_produced_blocks = $('.peer-produced-blocks-' + id);
                            
                            if (peer_state_span.text() === 'IDLE') {
                                peer_state_span.text('BUSY');
                                peer_name.addClass('font-weight-bold');
                                peer_state.addClass('peer-state-busy');
                                peer_state.closest('tr').hide().fadeIn(500);
                            }

                            $('.peer-state span').not(peer_state_span).text('IDLE');
                            $('.peer-name').not(peer_name).removeClass('font-weight-bold');
                            $('.peer-state').not(peer_state).removeClass('peer-state-busy');

                            peer_produced_blocks.attr('number', parseInt(peer_produced_blocks.attr('number')) + 1);
                            peer_produced_blocks.text(parseInt(peer_produced_blocks.attr('number')).toLocaleString());
                            
                            
                        });
                    }
                }
                // ******************************
                // * Handle BlockProducedSignal *
                // ******************************
                if (message.cmd === 'BlockProducedSignal') {
                    var leader_block_elapse = '';
                    var leader_block_remain = '';
                    if (leader_block_mcount < 10) {
                        leader_block_mcount += 1;
                    }
                    for (m = 0; m < leader_block_mcount; m++) {
                        leader_block_elapse += '❚';
                    }
                    for (r = leader_block_mcount; r < 10; r++) {
                        leader_block_remain += '❚';
                    }
                    $('.leader-block-mcount').text(leader_block_mcount);
                    $('.leader-block-elapse').text(leader_block_elapse);
                    $('.leader-block-remain').text(leader_block_remain);
                    if ($('#block_grid .e-content tr').length > 1) {
                        if ($('#block_grid .e-content tr').length >= 22) {
                            $('#block_grid .e-content tr:last').remove();
                        }
                        var row = $('#block_grid .e-content tr:first').clone().hide().css('opacity', 0);
                        $(row).find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4, true);
                        $(row).find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                        $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                        $(row).find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                        row.prependTo($('#block_grid .e-content tbody:first')).slideDown(150).animate({ opacity: 1 }, { queue: true, duration: 300 });
                    }
                    json.transactions.forEach(function(transaction) {
                        if ($('#transaction_grid .e-content tr').length > 1) {
                            if ($('#transaction_grid .e-content tr').length >= 22) {
                                $('#transaction_grid .e-content tr:last').remove();
                            }
                            var row = $('#transaction_grid .e-content tr:first').clone().hide().css('opacity', 0.25);
                            $(row).find('.TransactionResponse_Amount').decimal(transaction.amount, 4, true);
                            $(row).find('.TransactionResponse_To').text(transaction.to.substring(0, 10) + '..');
                            $(row).find('.TransactionResponse_From').text(transaction.from.substring(0, 10) + '..');
                            $(row).find('.TransactionResponse_Hash').text(transaction.hash.substring(0, 16) + '..');
                            row.prependTo($('#transaction_grid .e-content tbody:first')).slideDown(150).animate({ opacity: 1 }, { queue: true, duration: 300 });
                        }
                    });
                    console.log("Block:" + $('#block_grid .e-content tr').length);
                    console.log("Transaction:" + $('#transaction_grid .e-content tr').length);
                }
            }
        }
    });
});