function confirmDelete(uniquid, isDeleteClick) {
    var deleteSpan = 'deleteSpan_' + uniquid;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniquid;

    if (isDeleteClick) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeleteSpan).show();
    }
    else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
    }
}