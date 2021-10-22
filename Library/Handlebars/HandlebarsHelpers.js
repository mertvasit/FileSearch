// for loop
Handlebars.registerHelper('times', function (num, block) {
    var accum = '';
    for (var i = 1; i <= num; i++) {
        accum += block.fn(i);
    }
    return accum;
});

// compare
Handlebars.registerHelper('compare', function (str1, str2, options) {
    if (str1 == str2)
        return options.fn(this);
    else
        return options.inverse(this);
});

// increase
Handlebars.registerHelper("inc", function (value)
{
    return parseInt(value) + 1;
});