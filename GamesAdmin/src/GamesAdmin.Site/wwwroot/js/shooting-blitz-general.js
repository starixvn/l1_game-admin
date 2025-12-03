// ShootingBlitz General Settings JavaScript (Teams Management)

var shootingBlitz = (function () {
    var teams = [];

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
        var tabs = ['general', 'teams'];
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
        showCreateTeamModal: showCreateTeamModal,
        addPlayerRow: addPlayerRow,
        editTeam: editTeam,
        saveTeam: saveTeam,
        deleteTeam: deleteTeam
    };
})();

