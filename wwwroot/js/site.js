// Write your Javascript code.

$(function () {
    //Keep service checkbox in sync with unit input
    $(":checkbox[id*=Enabled]").change(function () {
        console.log("aha");

        var unit = this.id.replace("Enabled", "Units");
        if (this.checked) {
            $("#" + unit).removeAttr("disabled");
        }
        else {
            $("#" + unit).attr("disabled", "disabled")
        }
    })

    $("#printBill").click(function () {
       window.print();
    })
})