var clock;
function startTime(oldYear, oldMonth, oldDay, oldHours, oldMinutes, oldSeconds) {
    var day1 = new Date(oldYear, oldMonth - 1, oldDay, oldHours, oldMinutes, oldSeconds);

    var day3 = new Date();
    var totalSeconds = (day3 - day1) / 1000;

    var result = "";

    var hoursTime = Math.floor(totalSeconds / 3600);
    var hoursText = hoursTime.toString();
    hoursText = hoursText.concat(" time");
    if (hoursTime > 1 || hoursTime == 0) {
        hoursText = hoursText.concat("r");
    }

    totalSeconds %= 3600;
    var minutesTime = Math.floor(totalSeconds / 60);
    var minutesText = minutesTime.toString();
    minutesText = minutesText.concat(" minut");
    if (minutesTime > 1 || minutesTime == 0) {
        minutesText = minutesText.concat("ter");
    }

    var secondsTime = Math.round(totalSeconds % 60);
    var secondsText = secondsTime.toString();
    secondsText = secondsText.concat(" sekund");
    if (secondsTime > 1 || secondsTime == 0) {
        secondsText = secondsText.concat("er");
    }

    result = hoursText + ", " + minutesText + ", " + secondsText;

    var testElement = document.getElementsByClassName("startTime");
    for (var i = 0; i < testElement.length; i++) {
        testElement[i].innerHTML = result;
    }

    clock = setTimeout(startTime.bind(null, oldYear, oldMonth, oldDay, oldHours, oldMinutes, oldSeconds), 1000);
}
function stopTime() {
    clearTimeout(clock);
}