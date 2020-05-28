function confirmDelete(uniquid, isDeleteClick) {
    var deleteSpan = 'deleteSpan_' + uniquid;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniquid;
    var msgid = 'msg_' + uniquid;
    if (isDeleteClick) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeleteSpan).show();
        $("#" + msgid).show();
    }
    else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
        $("#" + msgid).hide();
    }
}