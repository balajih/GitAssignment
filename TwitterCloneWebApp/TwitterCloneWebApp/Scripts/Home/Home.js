
$(document).ready(function () {
    $("#btnUpdate").click(function () {
        var msg = $("#tweetMsg").val();
        var user = $("#hdnUserId").val();
        var today = new Date();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

        if (msg != "") {
            var request = $.ajax({
                url: updateURL,
                dataType: 'json',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify({ msg: msg, userId: user }),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    //$('table > tbody > tr:first').before('<tr class="table-light"><td>' + user + '</td><td>' + msg + '</td><td>' + time + '</td></tr >');
                    $('<tr class="table-light"><td>' + user + '</td><td>' + msg + '</td><td>' + time + '</td></tr >').prependTo('table > tbody');
                    $("#tweetMsg").val("");
                    var tweetsCount = parseInt($("#noOfTweets").html());
                    tweetsCount = tweetsCount + 1;
                    $("#noOfTweets").html(tweetsCount);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    alert("Unable to updat the tweet");
                }
            });
        }
        else {
            alert("Please add a tweet msg to update");
        }
        return false;
    });
    
    $('#txtSearchProfile').on("keypress", function (e) {
        if (e.keyCode == 13) {

            var searchVal = $(this).val();
            if (searchVal != "") {
                $.ajax({
                    url: '/Account/SearchProfile?userId=' + searchVal,
                    dataType: 'json',
                    cache:false,
                    type: 'GET',
                    contentType: 'application/json',
                    processData: false,
                    success: function (data) {
                        if (data != "success")
                        {
                            $("#errorMsg").css({ "display": "block" });
                        }
                        else
                        {
                            $("#errorMsg").css({ "display": "none" });
                            window.location.href = '/Account/Profile?userName=' + searchVal;
                        }
                        
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        alert("Unable to search profiles.");
                    }
                });
            }
            return false; // prevent the button click from happening
        }
    });
});


