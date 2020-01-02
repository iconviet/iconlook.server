$(document).ready(function() {
    $('[data-toggle=tooltip]').tooltip({ delay: { show: 0 } });
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
                    if ($('#block_grid').length) {
                        if ($('#block_grid .e-content tr').length > 1) {
                            if ($('#block_grid .e-content tr').length >= 22) {
                                $('#block_grid .e-content tr:last').remove();
                            }
                            const block_row = $('#block_grid .e-content tr:first').clone();
                            block_row.find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4, true);
                            block_row.find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                            block_row.find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                            block_row.find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                            block_row.hide().css('opacity', 0).css('background-color', '#ffffe1');
                            block_row.prependTo($('#block_grid .e-content tbody:first'));
                            block_row.slideDown(150).animate({ opacity: 1 }, 450).animate({ queue: true, 'background-color': '#ffffff' }, 300);
                        }
                        $('#block_grid .e-content tr').toArray().forEach(function(item, index) {
                            $(item).attr('aria-rowindex', index);
                            $(item).attr('data-uid', 'grid-row' + index);
                        });
                    }
                    if ($('#transaction_grid').length) {
                        json.transactions.forEach(function(item, index) {
                            if ($('#transaction_grid .e-content tr').length > 1) {
                                if ($('#transaction_grid .e-content tr').length >= 22) {
                                    $('#transaction_grid .e-content tr:last').remove();
                                }
                                const trans_row = $('#transaction_grid .e-content tr:first').clone();
                                trans_row.attr('aria-rowindex', index).attr('data-uid', 'grid-row' + index);
                                trans_row.find('.TransactionResponse_Amount').decimal(item.amount, 4, true);
                                trans_row.find('.TransactionResponse_To').text(item.to.substring(0, 10) + '..');
                                trans_row.find('.TransactionResponse_From').text(item.from.substring(0, 10) + '..');
                                trans_row.find('.TransactionResponse_Hash').text(item.hash.substring(0, 16) + '..');
                                trans_row.hide().css('opacity', 0).css('background-color', '#fffff0');
                                trans_row.prependTo($('#transaction_grid .e-content tbody:first'));
                                trans_row.slideDown(150).animate({ opacity: 1 }, 450).animate({ queue: true, 'background-color': '#ffffff' }, 300);
                            }
                        });
                        $('#transactions_grid .e-content tr').toArray().forEach(function(item, index) {
                            $(item).attr('aria-rowindex', index);
                            $(item).attr('data-uid', 'grid-row' + index);
                        });
                    }
                }
            }
        }
    });
});