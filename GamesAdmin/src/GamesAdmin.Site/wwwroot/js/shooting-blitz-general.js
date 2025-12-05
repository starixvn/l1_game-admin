// ShootingBlitz General Settings JavaScript (Teams Management)
// Uses unified endpoint /shooting_blitz_config + ActionType

var shootingBlitz = (function () {
    var teams = [];

    // ActionType enum (corresponds to backend)
    var ActionType = {
        UpdateConfig: 0,
        QueryRtpConfigs: 1,
        CreateRtpConfig: 2,
        UpdateRtpConfig: 3,
        DeleteRtpConfig: 4,
        SelectRtpConfig: 5,
        QueryTeams: 6,
        CreateTeam: 7,
        UpdateTeam: 8,
        DeleteTeam: 9
    };

    function init() {
        var teamsDataEl = document.getElementById('teamsData');
        
        if (teamsDataEl && teamsDataEl.value) {
            try {
                teams = JSON.parse(teamsDataEl.value);
            } catch (e) {
                teams = [];
            }
        }
    }

    function toggleSettingTabs(tabName) {
        var tabs = ['general', 'teams', 'rtp'];
        tabs.forEach(function (tab) {
            // RTP tab uses 'rtp-tab-content' to avoid ID conflicts
            var elementId = tab === 'rtp' ? 'rtp-tab-content' : tab;
            var tabElement = document.getElementById(elementId);
            var tabHeader = document.getElementById(tab + '-tab-header');
            if (tabElement) {
                tabElement.style.display = tab === tabName ? 'block' : 'none';
            }
            if (tabHeader) {
                tabHeader.classList.toggle('active', tab === tabName);
            }
        });
    }

    // Team Functions
    function showCreateTeamModal() {
        document.getElementById('teamModalTitle').textContent = 'Create Team';
        document.getElementById('teamDbId').value = '';
        document.getElementById('teamId').value = '';
        document.getElementById('teamName').value = '';
        document.getElementById('teamIsEnabled').checked = true;  // Default enabled
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
        document.getElementById('teamIsEnabled').checked = team.isEnabled !== false;  // null/undefined treated as true
        document.getElementById('playerListContainer').innerHTML = '';
        
        if (team.starPlayList) {
            team.starPlayList.forEach(function (player) {
                addPlayerRow(player.name, player.playerNumber);
            });
        }
        
        $('#teamModal').modal('show');
    }

    function saveTeam() {
        var dbId = document.getElementById('teamDbId').value;
        var teamId = parseInt(document.getElementById('teamId').value);
        var teamName = document.getElementById('teamName').value;
        var isEnabled = document.getElementById('teamIsEnabled').checked;
        
        if (!teamId || !teamName) {
            showError('Team ID and Name are required');
            return;
        }
        
        var starPlayList = [];
        document.querySelectorAll('.player-row').forEach(function (row) {
            var name = row.querySelector('.player-name').value;
            var playerNumber = parseInt(row.querySelector('.player-number').value);
            if (name && !isNaN(playerNumber)) {
                starPlayList.push({ name: name, playerNumber: playerNumber });
            }
        });
        
        // Use unified endpoint with ActionType (per Cursor Rules 4.2)
        var data = {
            actionType: dbId ? ActionType.UpdateTeam : ActionType.CreateTeam,
            teamDbId: dbId || null,
            teamId: teamId,
            teamName: teamName,
            isEnabled: isEnabled,
            starPlayList: starPlayList
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    $('#teamModal').modal('hide');
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to save team');
                }
            },
            error: function (xhr) {
                showError('Failed to save team: ' + (xhr.responseText || 'Unknown error'));
            }
        });
    }

    function deleteTeam(id) {
        if (!confirm('Are you sure you want to delete this team?')) return;
        
        // Use unified endpoint with ActionType (per Cursor Rules 4.2)
        var data = {
            actionType: ActionType.DeleteTeam,
            teamDbId: id
        };
        
        $.ajax({
            url: '/shooting_blitz_config',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                if (result.success) {
                    showSuccess();
                    location.reload();
                } else {
                    showError(result.message || 'Failed to delete team');
                }
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

    // RTP Setting Functions
    function addRtpSettingRow(key, rtpConfigId) {
        var container = document.getElementById('rtpSettingContainer');
        var rtpConfigsDataEl = document.getElementById('rtpConfigsData');
        var rtpConfigs = [];
        
        if (rtpConfigsDataEl && rtpConfigsDataEl.value) {
            try {
                rtpConfigs = JSON.parse(rtpConfigsDataEl.value);
            } catch (e) {
                rtpConfigs = [];
            }
        }
        
        var optionsHtml = '<option value="">-- Select RTP Config --</option>';
        rtpConfigs.forEach(function (config) {
            var isSelected = rtpConfigId === config.id ? 'selected' : '';
            optionsHtml += '<option value="' + config.id + '" ' + isSelected + '>' + (config.name || '') + '</option>';
        });
        
        var html = '<div class="form-group row rtp-setting-row mb-2">' +
            '<div class="col-lg-4">' +
            '<label class="control-label">Key</label>' +
            '<input type="text" class="form-control rtp-setting-key" placeholder="e.g., normal, live, 1, 2" value="' + (key || '') + '" />' +
            '</div>' +
            '<div class="col-lg-6">' +
            '<label class="control-label">RTP Config</label>' +
            '<select class="form-control rtp-setting-config">' + optionsHtml + '</select>' +
            '</div>' +
            '<div class="col-lg-2">' +
            '<label class="control-label">&nbsp;</label>' +
            '<button type="button" class="btn btn-danger btn-sm btn-block" onclick="this.closest(\'.rtp-setting-row\').remove()">' +
            '<i class="icon-trash"></i> Remove' +
            '</button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
    }

    // Score Setting Functions
    function addScoreSettingRow(homeScore, awayScore, probability) {
        var container = document.getElementById('scoreSettingContainer');
        var html = '<div class="form-group row score-setting-row mb-2">' +
            '<div class="col-lg-3">' +
            '<label class="control-label">Home Score</label>' +
            '<input type="number" class="form-control score-setting-home" value="' + (homeScore || '0') + '" />' +
            '</div>' +
            '<div class="col-lg-3">' +
            '<label class="control-label">Away Score</label>' +
            '<input type="number" class="form-control score-setting-away" value="' + (awayScore || '0') + '" />' +
            '</div>' +
            '<div class="col-lg-4">' +
            '<label class="control-label">Probability (0-1)</label>' +
            '<input type="number" step="0.01" min="0" max="1" class="form-control score-setting-probability" value="' + (probability || '0') + '" onchange="shootingBlitz.updateProbabilitySum()" />' +
            '</div>' +
            '<div class="col-lg-2">' +
            '<label class="control-label">&nbsp;</label>' +
            '<button type="button" class="btn btn-danger btn-sm btn-block" onclick="this.closest(\'.score-setting-row\').remove(); shootingBlitz.updateProbabilitySum();">' +
            '<i class="icon-trash"></i> Remove' +
            '</button>' +
            '</div>' +
            '</div>';
        container.insertAdjacentHTML('beforeend', html);
        updateProbabilitySum();
    }

    function updateProbabilitySum() {
        var sum = 0;
        document.querySelectorAll('.score-setting-probability').forEach(function (input) {
            var value = parseFloat(input.value) || 0;
            sum += value;
        });
        
        var sumEl = document.getElementById('probabilitySum');
        if (sumEl) {
            sumEl.textContent = 'Total Probability: ' + sum.toFixed(2);
            if (Math.abs(sum - 1.0) > 0.01) {
                sumEl.style.color = '#dc3545'; // Red if not equal to 1.0
            } else {
                sumEl.style.color = '#28a745'; // Green if equal to 1.0
            }
        }
    }

    // Collect RTP Setting and Score Setting data for form submission
    function collectShootingBlitzConfig() {
        var rtpSettings = [];
        document.querySelectorAll('.rtp-setting-row').forEach(function (row) {
            var key = row.querySelector('.rtp-setting-key').value.trim();
            var rtpConfigId = row.querySelector('.rtp-setting-config').value;
            if (key && rtpConfigId) {
                rtpSettings.push({
                    key: key,
                    rtpConfigId: rtpConfigId
                });
            }
        });

        var scoreSettings = [];
        document.querySelectorAll('.score-setting-row').forEach(function (row) {
            var homeScore = parseInt(row.querySelector('.score-setting-home').value) || 0;
            var awayScore = parseInt(row.querySelector('.score-setting-away').value) || 0;
            var probability = parseFloat(row.querySelector('.score-setting-probability').value) || 0;
            scoreSettings.push({
                homeScore: homeScore,
                awayScore: awayScore,
                probability: probability
            });
        });

        var maxWinRound = parseFloat(document.getElementById('MaxWinRound').value) || 0;

        return {
            rtpSetting: rtpSettings,
            scoreSetting: scoreSettings,
            maxWinRound: maxWinRound
        };
    }

    // Initialize on document ready
    $(document).ready(function () {
        init();
        updateProbabilitySum();
        
        // Intercept form submission to include RTP Setting and Score Setting
        var form = document.getElementById('gameForm');
        if (form) {
            form.addEventListener('submit', function (e) {
                var config = collectShootingBlitzConfig();
                
                // Remove existing hidden inputs
                document.querySelectorAll('[name^="ShootingBlitzConfig."]').forEach(function (el) {
                    el.remove();
                });
                
                // Add MaxWinRound
                var maxWinRoundInput = document.createElement('input');
                maxWinRoundInput.type = 'hidden';
                maxWinRoundInput.name = 'ShootingBlitzConfig.MaxWinRound';
                maxWinRoundInput.value = config.maxWinRound;
                form.appendChild(maxWinRoundInput);
                
                // Add RTP Settings
                config.rtpSetting.forEach(function (rtpSetting, index) {
                    var keyInput = document.createElement('input');
                    keyInput.type = 'hidden';
                    keyInput.name = 'ShootingBlitzConfig.RtpSetting[' + index + '].Key';
                    keyInput.value = rtpSetting.key;
                    form.appendChild(keyInput);
                    
                    var rtpConfigIdInput = document.createElement('input');
                    rtpConfigIdInput.type = 'hidden';
                    rtpConfigIdInput.name = 'ShootingBlitzConfig.RtpSetting[' + index + '].RtpConfigId';
                    rtpConfigIdInput.value = rtpSetting.rtpConfigId;
                    form.appendChild(rtpConfigIdInput);
                });
                
                // Add Score Settings
                config.scoreSetting.forEach(function (scoreSetting, index) {
                    var homeScoreInput = document.createElement('input');
                    homeScoreInput.type = 'hidden';
                    homeScoreInput.name = 'ShootingBlitzConfig.ScoreSetting[' + index + '].HomeScore';
                    homeScoreInput.value = scoreSetting.homeScore;
                    form.appendChild(homeScoreInput);
                    
                    var awayScoreInput = document.createElement('input');
                    awayScoreInput.type = 'hidden';
                    awayScoreInput.name = 'ShootingBlitzConfig.ScoreSetting[' + index + '].AwayScore';
                    awayScoreInput.value = scoreSetting.awayScore;
                    form.appendChild(awayScoreInput);
                    
                    var probabilityInput = document.createElement('input');
                    probabilityInput.type = 'hidden';
                    probabilityInput.name = 'ShootingBlitzConfig.ScoreSetting[' + index + '].Probability';
                    probabilityInput.value = scoreSetting.probability;
                    form.appendChild(probabilityInput);
                });
            });
        }
    });

    return {
        toggleSettingTabs: toggleSettingTabs,
        showCreateTeamModal: showCreateTeamModal,
        addPlayerRow: addPlayerRow,
        editTeam: editTeam,
        saveTeam: saveTeam,
        deleteTeam: deleteTeam,
        addRtpSettingRow: addRtpSettingRow,
        addScoreSettingRow: addScoreSettingRow,
        updateProbabilitySum: updateProbabilitySum,
        collectShootingBlitzConfig: collectShootingBlitzConfig
    };
})();

