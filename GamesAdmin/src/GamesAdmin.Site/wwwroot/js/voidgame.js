function confirmVoid() {
    const memberId = $("#MemberId").val();
    const roundId = $("#GameRoundId").val();

    if (memberId && roundId) {
        let message = "Voiding Ticket with following information: \n";
        message += "MemberId: " + memberId + ", RoundId: " + roundId + "\n";
        message += "Are you sure you want to void this ticket? This action cannot be undone.";

        return confirm(message);
    }

    return false;
};