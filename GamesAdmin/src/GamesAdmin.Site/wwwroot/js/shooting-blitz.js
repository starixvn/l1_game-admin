// ShootingBlitz Game Settings JavaScript

var shootingBlitz = (function () {
    var rtpConfigs = [];
    var teams = [];
    var currentModeCount = 0;

    function init() {
        var rtpConfigsEl = document.getElementById('rtpConfigsData');
        var teamsDataEl = document.getElementById('teamsData');
        
        if (rtpConfigsEl && rtpConfigsEl.value) {
            try {
                rtpConfigs = JSON.parse(rtpConfigsEl.value);
            } catch (e) {
                rtpConfigs = [];
            }
        }
        
        if (teamsDataEl && teamsDataEl.value) {
            try {
                teams = JSON.parse(teamsDataEl.value);
            } catch (e) {
                teams = [];
            }
        }
    }

    function toggleSettingTabs(tabName) {
        var tabs = ['general', 'rtpconfig', 'teams'];
        tabs.forEach(function (tab) {
            var tabElement = document.getElementById(tab);
            var tabHeader = document.getElementById(tab + '-tab-header');
            if (tabElement) {
                tabElement.style.display = tab === tabName ? 'block' : 'none';
            }
            if (tabHeader) {
                tabHeader.classList.toggle('active', tab === tabName);
            }
        });
    }

    // RTP Config Functions
    function showCreateRtpModal() {
        document.getElementById('rtpConfigModalTitle').textContent = 'Create RTP Config';
        document.getElementById('rtpConfigId').value = '';
        document.getElementById('rtpValue').value = '0.95';
        document.getElementById('bombCountContainer').innerHTML = '';
        document.getElementById('multiplierStepsTabs').innerHTML = '';
        document.getElementById('multiplierStepsContent').innerHTML = '';
        currentModeCount = 0;
        
        // Add default bomb counts
        for (var i = 1; i <= 7; i++) {
            addBombCountRow(i, 0);
        }
        
        updateProbabilitySum();
        $('#rtpConfigModal').modal('show');
    }

    function addBombCountRow(bombCount, probability) {
        var container = document.getElementById('bombCountContainer');
        var index = container.children.length;
        var html = '<div class="form-group row bomb-count-row" data-index="' + index + '">' +
            '<div class="col-4">' +
            '<input type="number" class="form-control" placeholder="Bomb Count" value="' + (bombCount || '') + '" onchange="shootingBlitz.updateProbabilitySum()" />' +
            '</div>' +
            '<div class="col-4">' +
            '<input type="number" step="0.01" class="form-control probability-input" placeholder="Probability" value="' + (probability || '') + '" onchange="shootingBlitz.updateProbabilitySum()" />' +
            '</div>' +
            '<div class="col-4">' +
            '<button type="button" class="btn btn-danger btn-sm" onclick="shootingBlitz.removeBombCountRow(this)"><i class="icon-trash"></i></button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    function removeBombCountRow(btn) {
        btn.closest('.bomb-count-row').remove();
        updateProbabilitySum();
    }

    function updateProbabilitySum() {
        var inputs = document.querySelectorAll('.probability-input');
        var sum = 0;
        inputs.forEach(function (input) {
            sum += parseFloat(input.value) || 0;
        });
        document.getElementById('probabilitySum').textContent = sum.toFixed(4);
    }

    function addMultiplierMode(modeData) {
        currentModeCount++;
        var modeNum = modeData ? modeData.mode : currentModeCount;
        var tabId = 'mode-' + currentModeCount;
        
        // Add tab
        var tabsContainer = document.getElementById('multiplierStepsTabs');
        var isFirst = tabsContainer.children.length === 0;
        var tabHtml = '<li class="nav-item">' +
            '<a class="nav-link' + (isFirst ? ' active' : '') + '" data-toggle="tab" href="#' + tabId + '">Mode ' + modeNum + '</a>' +
            '</li>';
        tabsContainer.insertAdjacentHTML('beforeend', tabHtml);
        
        // Add tab content
        var contentContainer = document.getElementById('multiplierStepsContent');
        var contentHtml = '<div class="tab-pane' + (isFirst ? ' active' : '') + '" id="' + tabId + '" data-mode="' + modeNum + '">' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Mode Number</label>' +
            '<input type="number" class="form-control mode-number" value="' + modeNum + '" />' +
            '</div>' +
            '<div class="col-8">' +
            '<label>Name</label>' +
            '<input type="text" class="form-control mode-name" value="' + (modeData ? modeData.name : '') + '" />' +
            '</div>' +
            '</div>' +
            '<div class="form-group row">' +
            '<div class="col-4">' +
            '<label>Star Player Rate</label>' +
            '<input type="number" step="0.01" class="form-control star-player-rate" value="' + (modeData ? modeData.starPlayerRate : '0.02') + '" />' +
            '</div>' +
            '<div class="col-8">' +
            '<label>Star Position (comma separated)</label>' +
            '<input type="text" class="form-control star-position" value="' + (modeData && modeData.starPosition ? modeData.starPosition.join(',') : '') + '" />' +
            '</div>' +
            '</div>' +
            '<fieldset class="border p-2">' +
            '<legend class="w-auto">Multiplier Table</legend>' +
            '<div class="multiplier-table-container"></div>' +
            '<button type="button" class="btn btn-secondary btn-sm" onclick="shootingBlitz.addMultiplierRow(this)"><i class="icon-plus"></i> Add Row</button>' +
            '</fieldset>' +
            '<button type="button" class="btn btn-danger btn-sm mt-2" onclick="shootingBlitz.removeMode(this)"><i class="icon-trash"></i> Remove Mode</button>' +
            '</div>';
        contentContainer.insertAdjacentHTML('beforeend', contentHtml);
        
        // Add multiplier rows if data provided
        if (modeData && modeData.multiplier) {
            var tableContainer = contentContainer.lastElementChild.querySelector('.multiplier-table-container');
            modeData.multiplier.forEach(function (item) {
                addMultiplierRowToContainer(tableContainer, item.picks, item.pays);
            });
        }
    }

    function addMultiplierRow(btn) {
        var container = btn.previousElementSibling;
        addMultiplierRowToContainer(container, '', '');
    }

    function addMultiplierRowToContainer(container, picks, pays) {
        var html = '<div class="form-group row multiplier-row">' +
            '<div class="col-4">' +
            '<input type="number" class="form-control picks-input" placeholder="Picks" value="' + picks + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<input type="number" step="0.01" class="form-control pays-input" placeholder="Pays" value="' + pays + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<button type="button" class="btn btn-danger btn-sm" onclick="this.closest(\'.multiplier-row\').remove()"><i class="icon-trash"></i></button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    function removeMode(btn) {
        var tabPane = btn.closest('.tab-pane');
        var tabId = tabPane.id;
        var tabLink = document.querySelector('a[href="#' + tabId + '"]');
        
        if (tabLink) {
            tabLink.closest('li').remove();
        }
        tabPane.remove();
        
        // Activate first remaining tab
        var firstTab = document.querySelector('#multiplierStepsTabs .nav-link');
        var firstContent = document.querySelector('#multiplierStepsContent .tab-pane');
        if (firstTab && firstContent) {
            firstTab.classList.add('active');
            firstContent.classList.add('active');
        }
    }

    function editRtpConfig(id) {
        var config = rtpConfigs.find(function (c) { return c.id === id; });
        if (!config) return;
        
        document.getElementById('rtpConfigModalTitle').textContent = 'Edit RTP Config';
        document.getElementById('rtpConfigId').value = id;
        document.getElementById('rtpValue').value = config.rtp;
        document.getElementById('bombCountContainer').innerHTML = '';
        document.getElementById('multiplierStepsTabs').innerHTML = '';
        document.getElementById('multiplierStepsContent').innerHTML = '';
        currentModeCount = 0;
        
        // Load bomb counts
        if (config.bombCount) {
            config.bombCount.forEach(function (bc) {
                addBombCountRow(bc.bombCount, bc.probability);
            });
        }
        
        // Load multiplier steps
        if (config.multiplierSteps) {
            config.multiplierSteps.forEach(function (step) {
                addMultiplierMode(step);
            });
        }
        
        updateProbabilitySum();
        $('#rtpConfigModal').modal('show');
    }

    function saveRtpConfig() {
        var id = document.getElementById('rtpConfigId').value;
        var rtp = parseFloat(document.getElementById('rtpValue').value);
        
        // Collect bomb counts
        var bombCounts = [];
        document.querySelectorAll('.bomb-count-row').forEach(function (row) {
            var inputs = row.querySelectorAll('input');
            if (inputs[0].value && inputs[1].value) {
                bombCounts.push({
                    bombCount: parseInt(inputs[0].value),
                    probability: parseFloat(inputs[1].value)
                });
            }
        });
        
        // Collect multiplier steps
        var multiplierSteps = [];
        document.querySelectorAll('#multiplierStepsContent .tab-pane').forEach(function (pane) {
            var mode = parseInt(pane.querySelector('.mode-number').value);
            var name = pane.querySelector('.mode-name').value;
            var starPlayerRate = parseFloat(pane.querySelector('.star-player-rate').value);
            var starPositionStr = pane.querySelector('.star-position').value;
            var starPosition = starPositionStr ? starPositionStr.split(',').map(function (x) { return parseInt(x.trim()); }) : [];
            
            var multiplier = [];
            pane.querySelectorAll('.multiplier-row').forEach(function (row) {
                var picks = parseInt(row.querySelector('.picks-input').value);
                var pays = parseFloat(row.querySelector('.pays-input').value);
                if (!isNaN(picks) && !isNaN(pays)) {
                    multiplier.push({ picks: picks, pays: pays });
                }
            });
            
            // Sort by picks ascending
            multiplier.sort(function (a, b) { return a.picks - b.picks; });
            
            multiplierSteps.push({
                mode: mode,
                name: name,
                starPlayerRate: starPlayerRate,
                starPosition: starPosition,
                multiplier: multiplier
            });
        });
        
        var data = {
            rtp: rtp,
            bombCount: bombCounts,
            multiplierSteps: multiplierSteps
        };
        
        var url = id ? '/api/shooting_blitz_rtp/' + id : '/api/shooting_blitz_rtp';
        var method = id ? 'PUT' : 'POST';
        
        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                $('#rtpConfigModal').modal('hide');
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to save RTP config: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteRtpConfig(id) {
        if (!confirm('Are you sure you want to delete this RTP config?')) return;
        
        $.ajax({
            url: '/api/shooting_blitz_rtp/' + id,
            method: 'DELETE',
            success: function () {
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to delete RTP config: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    // Team Functions
    function showCreateTeamModal() {
        document.getElementById('teamModalTitle').textContent = 'Create Team';
        document.getElementById('teamDbId').value = '';
        document.getElementById('teamId').value = '';
        document.getElementById('teamName').value = '';
        document.getElementById('playerListContainer').innerHTML = '';
        
        $('#teamModal').modal('show');
    }

    function addPlayerRow(name, playerNumber) {
        var container = document.getElementById('playerListContainer');
        var html = '<div class="form-group row player-row">' +
            '<div class="col-5">' +
            '<input type="text" class="form-control player-name" placeholder="Player Name" value="' + (name || '') + '" />' +
            '</div>' +
            '<div class="col-4">' +
            '<input type="number" class="form-control player-number" placeholder="Jersey Number" value="' + (playerNumber || '') + '" />' +
            '</div>' +
            '<div class="col-3">' +
            '<button type="button" class="btn btn-danger btn-sm" onclick="this.closest(\'.player-row\').remove()"><i class="icon-trash"></i></button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    function editTeam(id) {
        var team = teams.find(function (t) { return t.id === id; });
        if (!team) return;
        
        document.getElementById('teamModalTitle').textContent = 'Edit Team';
        document.getElementById('teamDbId').value = id;
        document.getElementById('teamId').value = team.teamId;
        document.getElementById('teamName').value = team.name;
        document.getElementById('playerListContainer').innerHTML = '';
        
        if (team.playerList) {
            team.playerList.forEach(function (player) {
                addPlayerRow(player.name, player.playerNumber);
            });
        }
        
        $('#teamModal').modal('show');
    }

    function saveTeam() {
        var dbId = document.getElementById('teamDbId').value;
        var teamId = parseInt(document.getElementById('teamId').value);
        var teamName = document.getElementById('teamName').value;
        
        if (!teamId || !teamName) {
            showError('Team ID and Name are required');
            return;
        }
        
        var playerList = [];
        document.querySelectorAll('.player-row').forEach(function (row) {
            var name = row.querySelector('.player-name').value;
            var playerNumber = parseInt(row.querySelector('.player-number').value);
            if (name && !isNaN(playerNumber)) {
                playerList.push({ name: name, playerNumber: playerNumber });
            }
        });
        
        var data = {
            teamId: teamId,
            name: teamName,
            playerList: playerList
        };
        
        var url = dbId ? '/api/shooting_blitz_team/' + dbId : '/api/shooting_blitz_team';
        var method = dbId ? 'PUT' : 'POST';
        
        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                $('#teamModal').modal('hide');
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to save team: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteTeam(id) {
        if (!confirm('Are you sure you want to delete this team?')) return;
        
        $.ajax({
            url: '/api/shooting_blitz_team/' + id,
            method: 'DELETE',
            success: function () {
                showSuccess();
                location.reload();
            },
            error: function (xhr) {
                showError('Failed to delete team: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function showSuccess() {
        var successMsg = document.getElementById('successMsg');
        if (successMsg) {
            successMsg.classList.remove('hidden-cls');
            setTimeout(function () {
                successMsg.classList.add('hidden-cls');
            }, 3000);
        }
    }

    function showError(message) {
        var errorMsg = document.getElementById('failedMsg');
        if (errorMsg) {
            errorMsg.querySelector('span').textContent = message || 'Error';
            errorMsg.classList.remove('hidden-cls');
            setTimeout(function () {
                errorMsg.classList.add('hidden-cls');
            }, 5000);
        }
    }

    // Initialize on document ready
    $(document).ready(function () {
        init();
    });

    return {
        toggleSettingTabs: toggleSettingTabs,
        showCreateRtpModal: showCreateRtpModal,
        addBombCountRow: addBombCountRow,
        removeBombCountRow: removeBombCountRow,
        updateProbabilitySum: updateProbabilitySum,
        addMultiplierMode: addMultiplierMode,
        addMultiplierRow: addMultiplierRow,
        removeMode: removeMode,
        editRtpConfig: editRtpConfig,
        saveRtpConfig: saveRtpConfig,
        deleteRtpConfig: deleteRtpConfig,
        showCreateTeamModal: showCreateTeamModal,
        addPlayerRow: addPlayerRow,
        editTeam: editTeam,
        saveTeam: saveTeam,
        deleteTeam: deleteTeam
    };
})();

