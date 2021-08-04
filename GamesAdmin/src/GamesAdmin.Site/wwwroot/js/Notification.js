function DeleteButton(id) {
    var isDelete = confirm("Do you want to delete this item?");
    if (isDelete) {
        $.ajax({
            method: "POST",
            url: "/Notification/Delete",
            data: { id: id }
        }).done(function (updated) {
            if (updated) {
                window.location.href = "/notification";
            }
        });
    }
}
//# sourceMappingURL=Notification.js.map