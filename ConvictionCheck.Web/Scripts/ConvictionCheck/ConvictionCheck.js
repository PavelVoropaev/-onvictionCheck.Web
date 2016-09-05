$("input[name='IsHaveConviction']:radio").change(function () {
    if (this.value === "True") {
        $("#ArticleNumberBlock").toggle(true);
    } else {
        $("#ArticleNumberBlock").toggle(false);

    }
});