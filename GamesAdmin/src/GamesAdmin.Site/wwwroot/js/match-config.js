var matchConfig = (function ($) {
    var $matchConfigTemplate;
    var matchConfigIndex;
    return {
        init: function () {
            console.log("init match config");
            matchConfigIndex = $("#match-configs .match-item").length;

            $matchConfigTemplate = $(`<div class="row mt-2 match-item">
                <input type="hidden" class="id" id="matches_${matchConfigIndex}__Id" name="matches[${matchConfigIndex}].Id" value="">
                <label class="control-label col-1 mt-2 mr-2" for="matches_${matchConfigIndex}__Home">Home</label>
                <input type="text" class="form-control col-1 home" id="matches_${matchConfigIndex}__Home" name="matches[${matchConfigIndex}].Home" value="">
                <label class="control-label col-1 mt-2" for="matches_${matchConfigIndex}__Away">Away</label>
                <input type="text" class="form-control col-1 away" id="matches_${matchConfigIndex}__Away" name="matches[${matchConfigIndex}].Away" value="">
                <label class="control-label col-2 mt-2" for="matches_${matchConfigIndex}__StartTime">StartTime</label>
                <input type="text" class="form-control col-3 startTime valid" value="" data-val="true" data-val-required="The StartTime field is required." id="matches_${matchConfigIndex}__StartTime" name="matches[${matchConfigIndex}].StartTime" aria-describedby="matches_${matchConfigIndex}__StartTime-error" aria-invalid="false">
                <i class="icon-cancel text-error mt-3 ml-2" style="cursor: pointer" onclick="matchConfig.removeMatchItem(event)"></i>
            </div>`);
            $(".startTime").datetimepicker({ format: 'm/d/Y H:i', });

        },
        addMatchItem: function () {
            var $newItem = $matchConfigTemplate.clone();

            var startTime = $(".startTime", $newItem);
            startTime.datetimepicker({ format: 'm/d/Y H:i', });

            console.log($newItem.html());

            $("#match-configs").prepend($newItem);
            matchConfigIndex++;
        },
        removeMatchItem: function (event) {
            $(event.target).parent().remove();
        },
        updateMatchConfigsIndex: function () {
            var matchConfigItems = $(".match-item", "#match-configs");
            let matches = [];
            for (let index = 0; index < matchConfigItems.length; index++) {
                const match = {};
                var item = matchConfigItems[index];
                var $id = $(".id", item);
                match.id = $id.val()

                var $home = $(".home", item);
                $home.attr("name", $home.attr("name").replace(/\d+/, index));
                match.home = $home.val()

                var $away = $(".away", item);
                $away.attr("name", $away.attr("name").replace(/\d+/, index));
                match.away = $away.val()

                var $startTime = $(".startTime", item);
                $startTime.attr("name", $startTime.attr("name").replace(/\d+/, index));

                match.startTime = $startTime.datetimepicker('getValue');
                matches.push(match);
            }
            return matches;
        },
    }
}(jQuery));

$(document).ready(function () {

});