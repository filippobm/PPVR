function selectedTextFocus(inputId) {
    var input = $("#" + inputId);

    if (input.val() == null) {
        console.log("Elemento html com id =" + inputId + "não foi encontrado.");
    } else {
        // Multiply by 2 to ensure the cursor always ends up at the end;
        // Opera sometimes sees a carriage return as 2 characters.
        var strLength = input.val().length * 2;

        input.focus();
        input[0].setSelectionRange(0, strLength);
    }
}

function validateForm(formId, validationSummaryId) {

    var form = $("#" + formId);
    var validationSummary = $("#" + validationSummaryId);

    form.submit(function () {
        if (!$(this).valid()) {
            validationSummary.show();
        }
    });

    var isNotValid = $("#" + formId + " ul li").html() !== "";

    if (isNotValid) {
        validationSummary.show();
    } else {
        validationSummary.hide();
    }
}