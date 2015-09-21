
    $(document).ready(function () {
        select2Dropdown('tagstring-hdn', 'tagstring', 'Search for tags(s)', 'Tag/SearchTag', 'Tag/GetTag', true);
        select2Dropdown('company-hdn', 'company-id', 'Search for company', 'Tag/SearchCompany', 'Tag/GetCompany', false);
    });
 
function select2Dropdown(hiddenID, valueID, ph, listAction, getAction, isMultiple) {
    var sid = '#' + hiddenID;
    $(sid).select2({
        placeholder: ph,
        minimumInputLength: 2,
        allowClear: true,
        multiple: isMultiple,
        //tags: ["red", "green", "blue"],
        //tokenSeparators: [",", " "],
        ajax: {
            url: "/api/" + listAction,
            dataType: 'json',
            data: function (term, page) {
                return {
                    id: term // search term
                };
            },
            results: function (data) {
                return { results: data };
            }
        },
        //Allow manually entered text in drop down.
        createSearchChoice: function (term, data) {
            if ($(data).filter(function () {
              return this.text.localeCompare(term) === 0;
            }).length === 0) {
                return { id: term, text: term };
            }
        },
        initSelection: function (element, callback) {
            // the input tag has a value attribute preloaded that points to a preselected tag's id
            // this function resolves that id attribute to an object that select2 can render
            // using its formatResult renderer - that way the make text is shown preselected
            var id = $('#' + valueID).val();
            if (id !== null && id.length > 0) {
                $.ajax("/api/" + getAction + "/" + id, {
                    dataType: "json"
                }).done(function (data) {
                    if (data.length < 2) {
                        callback(data[0]);
                    } else {
                        callback(data);
                    }
                    
                });
            }
        },
        formatResult: s2FormatResult,
        formatSelection: s2FormatSelection
    });
 

    $(document.body).on("change", sid, function (ev) {
        var choice;
        var values = ev.val;
        
        if (ev.val != "") {

            if (ev.target == document.getElementById("company-hdn")) {

                if ($.isNumeric(values)) {
                    choice = values;
                } else {
                    $.ajax("/api/Tag/CreateCompany/" + values, {
                        dataType: "json",
                        type: "POST",
                        success: function(data) {
                            choice = data;
                            $('#' + valueID).val(choice);
                        }
                    }).done(function(data) { choice = data; });
                }
            } else {
                // This is assuming the value will be an array of strings.
                // Convert to a comma-delimited string to set the value.
                if (values !== null && values.length > 0) {
                    for (var i = 0; i < values.length; i++) {
                        if (typeof choice !== 'undefined') {
                            choice += ",";
                            choice += values[i];
                        } else {
                            choice = values[i];
                        }
                    }
                }
            }


            // Set the value so that MVC will load the form values in the postback.
            $('#' + valueID).val(choice);
        } else {
            $('#' + valueID).val(0);
        }

        
        
    });
}
 
function s2FormatResult(item) {
    return item.text;
}
 
function s2FormatSelection(item) {
    return item.text;
}
