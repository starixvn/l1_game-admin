function DeleteButton(id) {
    var isDelete = confirm("Do you want to delete this item?");
    if (isDelete) {
        $.ajax({
            method: "POST",
            url: "/Notification/Delete",
            data: { id: id }
        }).done(function (updated) {
            if (!updated) {
            }
        });
    }
}
//# sourceMappingURL=Notification.js.map