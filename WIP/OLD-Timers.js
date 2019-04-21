function updateAllEvents() {
    $(".event").each(function () {
        var a, e, t, s, r, i, o, m, n, d, l;
        if (a = $(this), t = parseInt(a.data("type"), 10), e = a.find(".countdown"), 1 == t || 2 == t || 4 == t || 5 == t)
            for (s = parseInt(a.data("startday"), 10), r = parseInt(a.data("starttime"), 10), i = parseInt(a.data("endtime"), 10), n = i - r, o = moment().utc().subtract(tzOffset, "m").startOf("week").add(s, "d").add(r, "s"), m = moment(o), m.add(n, "s") ; m < moment().utc().subtract(tzOffset, "m") ;) o.add(7, "d"), m.add(7, "d");
        3 == t && (r = parseInt(a.data("starttime"), 10), i = parseInt(a.data("endtime"), 10), o = moment().utc().subtract(tzOffset, "m").startOf("day").add(r, "s"), n = i - r, m = moment(o), m.add(n, "s"), l = m.diff(moment().utc().subtract(tzOffset, "m"), "s"), l < 0 && (o.add(1, "d"), m.add(1, "d")), s = o.day(), a.find(".start-day").html(dayOfWeekAsString(s))), d = o.diff(moment().utc().subtract(tzOffset, "m"), "s"), $(a).data("countdown", d), $(a).data("duration", n)
    }), $(".timer-table tbody").each(function () {
        if ($(this).children(".event").sort(function (a, e) {
                var t, s;
                return t = parseInt($(a).data("countdown"), 10), s = parseInt($(e).data("countdown"), 10), t - s
        }).prependTo($(this)), "Show all events" == $(this).find(".show-all a").html()) var a = !1;
        else var a = !0;
        $(this).children(".event").each(function (e) {
            var t, s, r, i, o, m;
            if (t = $(this).data("countdown"), s = $(this).data("duration"), r = $(this).find(".countdown"), i = moment().utc().add(t, "s").unix(), o = moment().utc().add(t, "s").add(s, "s").unix(), i < moment().utc().unix() && moment().utc().unix() < o ? (m = !0, $(this).addClass("active")) : (m = !1, $(this).removeClass("active")), m) r.countdown(1e3 * o, function (a) {
                var e = 24 * a.offset.totalDays + a.offset.hours;
                $(this).html(a.strftime(e + ":%M:%S"))
            });
            else if (e < 5) $(this).hasClass("hide-event") && $(this).removeClass("hide-event"), r.countdown(1e3 * i, function (a) {
                var e = 24 * a.offset.totalDays + a.offset.hours;
                $(this).html(a.strftime(e + ":%M:%S"))
            });
            else {
                a || $(this).addClass("hide-event");
                var t = $(this).data("countdown");
                r.html(secondsToHms(t))
            }
        });
        var e = $(this).find(".event").first(),
            t = e.data("name"),
            s = e.data("img"),
            r = e.data("countdown"),
            i = e.data("duration"),
            o = parseInt(e.data("type"), 10),
            m = '<div class="first-event" style="background-image: url(\'' + s + "')\">";
        m += '<h1 class="first-name">' + t.replace(/</g, "") + "</h1>", 5 == o && (m += '<h2 class="first-info">Bait: ', m += e.data("baiturl") ? '<a class="secret" href="/item/' + e.data("baiturl") + '">' + e.data("bait") + "</a>" : e.data("bait"), m += "</h2>"), m += e.hasClass("active") ? '<div class="first-timer active" data-countdown="' + r + '" data-duration="' + i + '"></div>' : '<div class="first-timer" data-countdown="' + r + '"></div>', m += "</div>", $(this).parent().parent().find(".first-event").replaceWith(m)
    }), $(".first-timer").each(function (a) {
        var e = $(this).parent().parent(),
            t = e.prop("id"),
            s = e.find(".volume-icon").first(),
            r = $(this).data("countdown"),
            i = $(this).data("duration"),
            o = moment().utc().add(r, "s").unix(),
            m = moment().utc().add(r, "s").add(i, "s").unix();
        alertActive[a] && r > 240 && r < 300 && (s.hasClass("fa-volume-up") && playAlert(t + "-warning"), alertActive[a] = !1), alertActive[a + timerCount] && r > -60 && r < 0 && ((s.hasClass("fa-volume-up") || s.hasClass("fa-volume-down")) && playAlert(t + "-active"), alertActive[a + timerCount] = !1), $(this).hasClass("active") ? (alertActive[a] = !0, alertActive[a + timerCount] = !0, $(this).countdown(1e3 * m, function (a) {
            var e = 24 * a.offset.totalDays + a.offset.hours;
            $(this).html("In Progress - " + a.strftime(e + ":%M:%S"))
        }).on("finish.countdown", function () {
            clearTimeout(timer), timer = setTimeout(updateAllEvents, 2e3)
        })) : $(this).countdown(1e3 * o, function (a) {
            var e = 24 * a.offset.totalDays + a.offset.hours;
            $(this).html(a.strftime(e + ":%M:%S"))
        }).on("finish.countdown", function () {
            clearTimeout(timer), timer = setTimeout(updateAllEvents, 2e3)
        })
    }), timer = setTimeout(updateAllEvents, 6e4)
}

function setAllEvents() {
    $.each(timers, function (a, e) {
        var t, s, r, i, o, m, n, d, l, p;
        t = [], r = e.name, i = r.replace(/\s/g, "").toLowerCase(), m = a, s = '<div id="' + i + '" class="timer-container">', s += '<div class="first-event"></div>', s += '<table class="timer-table all-center"><thead><tr>', s += '<th class="timer-header" colspan="4">' + r, localStorage, s += '<div style="position: relative">', s += "am/pm" == localStorage["timer-format" + a] ? '<i class="fa fa-clock-o timeformat-icon" aria-hidden="true" title="Toggle between 24H and a.m./p.m."></i>' : '<i class="fa fa-clock-o timeformat-icon 24h" aria-hidden="true" title="Toggle between 24H and a.m./p.m."></i>', s += "volume-down" == localStorage["timer-alert" + a] ? '<i class="fa fa-volume-down volume-icon" aria-hidden="true" title="Current setting: Play sound alert when an event starts."></i>' : "volume-up" == localStorage["timer-alert" + a] ? '<i class="fa fa-volume-up volume-icon" aria-hidden="true" title="Current setting: Play sound alert when an event starts and 5 minutes in advance."></i>' : '<i class="fa fa-volume-off volume-icon" aria-hidden="true" title="Current setting: Mute all sound alerts."></i>', s += "</div></th>", s += "</tr></thead><tbody>", $.each(e.events, function (a, e) {
            if (o = e.type, 1 == o && (n = e.startDay, d = e.startTime, l = e.endDay, p = e.endTime, s += '<tr class="event" data-type="' + o + '" data-startday="' + n + '" data-endday="' + l + '" data-starttime="' + d + '" data-endtime="' + p + '" data-countdown="" data-img="' + e.img + '" data-name="' + e.eventName + '">', s += '<td class="countdown"></td>', s += '<td class="start-day">' + dayOfWeekAsString(n) + "</td>", localStorage, s += "am/pm" == localStorage["timer-format" + m] ? '<td class="start-time">' + moment().startOf("day").seconds(d).format("h:mm:ss a") + "</td>" : '<td class="start-time">' + moment().startOf("day").seconds(d).format("H:mm:ss") + "</td>", s += '<td class="event-name"><a href="/npc/' + e.eventUrl + '">' + e.eventName + "</a>", null != e.coords && (s += " " + e.coords), s += "</td></tr>"), 2 == o || 4 == o || 5 == o) {
                d = e.startTime, p = e.endTime;
                for (var a = 0, t = e.startDays.length; a < t; a++)
                    for (var r = e.startDays[a], i = 0, g = e.startTimes.length; i < g; i++) s += '<tr class="event" data-type="' + o + '" data-startday="' + r + '" data-endday="' + r + '" data-starttime="' + e.startTimes[i] + '" data-endtime="' + e.endTimes[i] + '" data-countdown="" data-img="' + e.img + '" data-name="' + e.eventName + '"', 5 == o && (s += ' data-bait="' + e.baitName + '"', null != e.baitUrl && (s += ' data-baiturl="' + e.baitUrl + '"')), s += ">", s += '<td class="countdown"></td>', s += '<td class="start-day">' + dayOfWeekAsString(r) + "</td>", localStorage, s += "am/pm" == localStorage["timer-format" + m] ? '<td class="start-time">' + moment().startOf("day").seconds(e.startTimes[i]).format("h:mm:ss a") + "</td>" : '<td class="start-time">' + moment().startOf("day").seconds(e.startTimes[i]).format("H:mm:ss") + "</td>", 2 == o ? s += '<td class="event-name">' + e.eventName + "</td></tr>" : (s += '<td class="event-name">', s += null != e.eventUrl ? '<a href="/npc/' + e.eventUrl + '">' + e.eventName + "</a>" : e.eventName, null != e.coords && (s += " " + e.coords), s += "</td></tr>")
            }
            if (3 == o)
                for (var i = 0, g = e.startTimes.length; i < g; i++) s += '<tr class="event" data-type="' + o + '" data-starttime="' + e.startTimes[i] + '" data-endtime="' + e.endTimes[i] + '" data-countdown="" data-img="' + e.img + '" data-name="' + e.eventName + '">', s += '<td class="countdown"></td>', s += '<td class="start-day"></td>', localStorage, s += "am/pm" == localStorage["timer-format" + m] ? '<td class="start-time">' + moment().startOf("day").seconds(e.startTimes[i]).format("h:mm:ss a") + "</td>" : '<td class="start-time">' + moment().startOf("day").seconds(e.startTimes[i]).format("H:mm:ss") + "</td>", s += '<td class="event-name"><span data-tooltip class="has-tip top" title="' + e.tooltip + '" data-options="allowHtml: true" data-template-classes="timer-tooltip">' + e.eventName + "</span></td>"
        }), s += '<tr class="show-all"><td colspan="4"><a href="#" class="more-link">Show all events</a></td></tr>', s += "</tbody></table></div>", t.push(s), $("<div/>", {
            "class": "column",
            html: t.join("")
        }).appendTo("#timers .row")
    })
}

function dayOfWeekAsString(a) {
    return ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][a]
}

function secondsToHms(a) {
    var e = Math.floor(a / 3600),
        t = Math.floor((a - 3600 * e) / 60),
        s = a - 3600 * e - 60 * t;
    s = Math.round(100 * s) / 100;
    var r = e < 10 ? "0" + e : e;
    return r += ":" + (t < 10 ? "0" + t : t), r += ":" + (s < 10 ? "0" + s : s)
}

function showAll() {
    $(".show-all a").click(function (a) {
        a.preventDefault(), $(a.target).hasClass("less-link") ? ($(a.target).html("Show all events"), $(a.target).removeClass("less-link"), $(a.target).addClass("more-link"), $(a.target).parents().eq(2).find("tr:gt(4)").not(".show-all").addClass("hide-event")) : ($(a.target).html("Show less events"), $(a.target).addClass("less-link"), $(a.target).removeClass("more-link"), $(a.target).parents().eq(2).find("tr:gt(4)").removeClass("hide-event"))
    })
}

function updateTime() {
    $(".server-time .clock").html(moment().utcOffset(-tzOffset).format("h:mm:ss A"))
}

function playAlert(a) {
    var e = document.getElementById("alertBox");
    0 == e.children.length && ($('<audio id="alertSound"><source src="' + imgPath + "/sounds/timers/" + a + '.mp3" type="audio/mp3"><source src="' + imgPath + "/sounds/timers/" + a + '.ogg" type="audio/ogg"><p>Your browser does not support the <code>audio</code> element </p></audio>').appendTo("#alertBox"), document.getElementById("alertSound").play(), document.querySelector("#alertSound").addEventListener("ended", function () {
        $("#alertBox").html("")
    }, !1))
}

function setTZOffsetTo() {
    if ((new Date).getUTCMonth() >= 3 && (new Date).getUTCMonth() <= 9) tzOffset = 240;
    else if (2 == (new Date).getUTCMonth()) {
        var a = new Date;
        a.setUTCMonth(2), a.setUTCDate(1);
        var e = a.getUTCDay(),
            t = 0;
        t = 0 != e ? (7 - e) % 7 : 0;
        var s = a.getUTCDate() + t,
            r = s + 7;
        tzOffset = (new Date).getUTCDate() == r ? (new Date).getUTCHours() - 5 > 2 ? 240 : 300 : (new Date).getUTCDate() > r ? 240 : 300
    } else if (10 == (new Date).getUTCMonth()) {
        var a = new Date;
        a.setUTCMonth(10), a.setUTCDate(1);
        var e = a.getUTCDay(),
            t = 0;
        t = 0 != e ? (7 - e) % 7 : 0;
        var s = a.getUTCDate() + t;
        tzOffset = (new Date).getUTCDate() < s ? 240 : (new Date).getUTCDate() == s ? (new Date).getUTCHours() - 4 > 2 ? 300 : 240 : 300
    } else tzOffset = 300;
    return tzOffset
}
$(document).ready(function () {
    updateAllEvents(), setInterval(updateTime, 1e3), showAll(), $(".volume-icon").click(function () {
        var a = $(".volume-icon").index($(this));
        $(this).hasClass("fa-volume-up") ? ($(this).removeClass("fa-volume-up"), $(this).addClass("fa-volume-off"), $(this).prop("title", "Current setting: Mute all sound alerts."), localStorage.removeItem("timer-alert" + a)) : $(this).hasClass("fa-volume-off") ? ($(this).removeClass("fa-volume-off"), $(this).addClass("fa-volume-down"), $(this).prop("title", "Current setting: Play sound alert when an event starts."), localStorage.setItem("timer-alert" + a, "volume-down")) : $(this).hasClass("fa-volume-down") && ($(this).removeClass("fa-volume-down"), $(this).addClass("fa-volume-up"), $(this).prop("title", "Current setting: Play sound alert when an event starts and 5 minutes in advance."), localStorage.setItem("timer-alert" + a, "volume-up"))
    }), $(".timeformat-icon").click(function () {
        var a = $(".timeformat-icon").index($(this));
        if ($(this).hasClass("24h")) {
            $(this).removeClass("24h");
            var e = $(this).closest("table").find("tbody").first();
            e.find("tr").not(".show-all").each(function () {
                var a = $(this).data("starttime");
                $(this).find(".start-time").html(moment().startOf("day").seconds(a).format("h:mm:ss a"))
            }), localStorage.setItem("timer-format" + a, "am/pm")
        } else {
            $(this).addClass("24h");
            var e = $(this).closest("table").find("tbody").first();
            e.find("tr").not(".show-all").each(function () {
                var a = $(this).data("starttime");
                $(this).find(".start-time").html(moment().startOf("day").seconds(a).format("H:mm:ss"))
            }), localStorage.removeItem("timer-format" + a)
        }
    })
}), $(window).load(function () {
    $("#loading").addClass("hide"), $("#timers").removeClass("hide")
});
var tzOffset = setTZOffsetTo(),
    alertActive = [!0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0, !0],
    timerCount = 8,
    timer, imgPath = gon.global.asset_host,
    timers = [{
        name: "Fish Kings",
        events: [{
            eventName: "All Fish Kings",
            eventUrl: null,
            baitName: "Various Baits",
            baitUrl: null,
            coords: null,
            type: 5,
            startDays: [0],
            startTimes: [46800, 61200],
            endTimes: [48600, 63e3],
            img: imgPath + "/images/timers/sunhunters-vale.jpg"
        }, {
            eventName: "Eternal Wisdom Elder",
            eventUrl: "55041-eternal-wisdom-elder",
            baitName: "Shining Crystal Shell",
            baitUrl: "13680-shining-crystal-shell",
            coords: '<a data-open="showMap" data-x-coord="[609.387,609.387,609.387,609.387,609.387]" data-y-coord="[666.618,666.618,666.618,666.618,666.618]" data-mapimage="S016S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 609, Y: 666)</sup></a>',
            type: 5,
            startDays: [1],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/vultures-vale.jpg"
        }, {
            eventName: "Khaz Walid",
            eventUrl: "55121-khaz-walid",
            baitName: "Finest Fish Head",
            baitUrl: "15499-finest-fish-head",
            coords: '<a data-open="showMap" data-x-coord="[785.0,785.0,785.0,785.0,785.0]" data-y-coord="[403.0,403.0,403.0,403.0,403.0]" data-mapimage="S024S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 785, Y: 403)</sup></a>',
            type: 5,
            startDays: [1],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/desolate-valley.jpg"
        }, {
            eventName: "Fragrant Monstrosity",
            eventUrl: "55106-fragrant-monstrosity",
            baitName: "Rainbow Cricket",
            baitUrl: "14450-rainbow-cricket",
            coords: '<a data-open="showMap" data-x-coord="[83.0,83.0,83.0,83.0,83.0]" data-y-coord="[423.0,423.0,423.0,423.0,423.0]" data-mapimage="S012S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 83, Y: 423)</sup></a>',
            type: 5,
            startDays: [1],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/rainmist-reach.jpg"
        }, {
            eventName: "Lake Lurker",
            eventUrl: "55107-lake-lurker",
            baitName: "Freshly Grilled Sirloin",
            baitUrl: "14454-freshly-grilled-sirloin",
            coords: '<a data-open="showMap" data-x-coord="[195.0,195.0,195.0,195.0,195.0]" data-y-coord="[206.0,206.0,206.0,206.0,206.0]" data-mapimage="S013S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 195, Y: 206)</sup></a>',
            type: 5,
            startDays: [1],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/emerald-marsh.jpg"
        }, {
            eventName: "Ocean Tyrant",
            eventUrl: "55012-ocean-tyrant",
            baitName: "Prince Saury",
            baitUrl: "13024-prince-saury",
            coords: '<a data-open="showMap" data-x-coord="[825.003,825.003,825.003,825.07,825.003]" data-y-coord="[343.391,343.391,343.458,343.391,343.391]" data-mapimage="S002S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 825, Y: 343)</sup></a>',
            type: 5,
            startDays: [2],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/port-skandia.jpg"
        }, {
            eventName: "Submerged Starlight",
            eventUrl: "55119-submerged-starlight",
            baitName: "Megasquid",
            baitUrl: "15491-megasquid",
            coords: '<a data-open="showMap" data-x-coord="[630.274,630.274,630.274,630.274,630.274]" data-y-coord="[645.931,645.931,645.931,645.931,645.931]" data-mapimage="S022S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 630, Y: 645)</sup></a>',
            type: 5,
            startDays: [2],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/candetonn-hill.jpg"
        }, {
            eventName: "Hot-Tempered Overlord",
            eventUrl: "55017-hot-tempered-overlord",
            baitName: "Wriggling River Snail",
            baitUrl: "13029-wriggling-river-snail",
            coords: '<a data-open="showMap" data-x-coord="[158.339,158.339,158.306,158.339,158.313]" data-y-coord="[531.151,531.151,531.156,531.151,531.116]" data-mapimage="S007S" data-map-x-coord="1024" data-map-y-coord="1024" href="javascript:;"><sup>(X: 158, Y: 531)</sup></a>',
            type: 5,
            startDays: [2],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/triatio-highlands.jpg"
        }, {
            eventName: "Shadow of the Deep",
            eventUrl: "55013-shadow-of-the-deep",
            baitName: "Small Fluorescent Squid",
            baitUrl: "13025-small-fluorescent-squid",
            coords: '<a data-open="showMap" data-x-coord="[773.111,773.111,773.111,773.067,773.09]" data-y-coord="[136.119,136.119,136.119,136.059,136.071]" data-mapimage="S003S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 773, Y: 136)</sup></a>',
            type: 5,
            startDays: [3],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/helonia-coast.jpg"
        }, {
            eventName: "Defender of the Marsh",
            eventUrl: "55018-defender-of-the-marsh",
            baitName: "Fresh Marsh Shrimp",
            baitUrl: "13030-fresh-marsh-shrimp",
            coords: '<a data-open="showMap" data-x-coord="[765.237,765.237,765.121,765.081,765.237]" data-y-coord="[500.508,500.508,500.51,500.649,500.508]" data-mapimage="S008S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 765, Y: 500)</sup></a>',
            type: 5,
            startDays: [3],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/candeo-marsh.jpg"
        }, {
            eventName: "Unidentified Foreign Creature",
            eventUrl: "55108-unidentified-foreign-creature",
            baitName: "Coral Red Fish",
            baitUrl: "14458-coral-red-fish",
            coords: '<a data-open="showMap" data-x-coord="[112.631,112.564,112.642,112.654,112.493]" data-y-coord="[582.216,582.241,582.329,582.317,582.141]" data-mapimage="S018S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 112, Y: 582)</sup></a>',
            type: 5,
            startDays: [3],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/starstruck-plateau.jpg"
        }, {
            eventName: "Golden Blossom",
            eventUrl: "55014-golden-blossom",
            baitName: "Golden Petal",
            baitUrl: "13028-golden-petal",
            coords: '<a data-open="showMap" data-x-coord="[461.657,461.657,461.657,461.758,461.879]" data-y-coord="[556.843,556.843,556.843,556.842,556.745]" data-mapimage="S004S" data-map-x-coord="1024" data-map-y-coord="1024" href="javascript:;"><sup>(X: 461, Y: 556)</sup></a>',
            type: 5,
            startDays: [4],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/crescent-hill.jpg"
        }, {
            eventName: "Shadow Lancer",
            eventUrl: "55118-shadow-lancer",
            baitName: "Live Emperor Fish",
            baitUrl: "15487-live-emperor-fish",
            coords: '<a data-open="showMap" data-x-coord="[756.283,756.495,756.538,756.496,756.411]" data-y-coord="[333.587,333.672,333.418,333.249,333.418]" data-mapimage="S021S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 756, Y: 333)</sup></a>',
            type: 5,
            startDays: [4],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/port-morton.jpg"
        }, {
            eventName: "Rainbow Caller",
            eventUrl: "55019-rainbow-caller",
            baitName: "Giant Black Mayfly",
            baitUrl: "13031-giant-black-mayfly",
            coords: '<a data-open="showMap" data-x-coord="[467.625,467.625,467.625,467.625,467.625]" data-y-coord="[134.907,134.907,134.907,134.907,134.907]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 467, Y: 134)</sup></a>',
            type: 5,
            startDays: [4],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/ventos-prairie.jpg"
        }, {
            eventName: "Eternal Epicurean",
            eventUrl: "55185-eternal-epicurean",
            baitName: "Aqualord Redshade",
            baitUrl: "17286-aqualord-redshade",
            coords: '<a data-open="showMap" data-x-coord="[177.681,177.88,177.924,177.839,177.736]" data-y-coord="[764.164,764.189,764.194,764.295,764.152]" data-mapimage="S025S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 177, Y: 764)</sup></a>',
            type: 5,
            startDays: [4],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/tanglevine-cascades.jpg"
        }, {
            eventName: "Ancient Witness",
            eventUrl: "55015-ancient-witness",
            baitName: "Magic Fig",
            baitUrl: "13027-magic-fig",
            coords: '<a data-open="showMap" data-x-coord="[152.058,152.043,152.043,152.043,152.019]" data-y-coord="[705.141,705.145,705.145,705.145,705.184]" data-mapimage="S005S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 152, Y: 705)</sup></a>',
            type: 5,
            startDays: [5],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/cactakara-forest.jpg"
        }, {
            eventName: "Silent Hunter",
            eventUrl: "55186-silent-hunter",
            baitName: "Tyrant Nautilus",
            baitUrl: "17290-tyrant-nautilus",
            coords: '<a data-open="showMap" data-x-coord="[640.062,640.132,639.974,640.01,640.045]" data-y-coord="[221.479,221.507,221.401,221.503,221.516]" data-mapimage="S026S" data-map-x-coord="1024" data-map-y-coord="960" href="javascript:;"><sup>(X: 640, Y: 221)</sup></a>',
            type: 5,
            startDays: [5],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/sunhunters-vale.jpg"
        }, {
            eventName: "Deep Waterfall Ghost",
            eventUrl: "55020-deep-waterfall-ghost",
            baitName: "Green Leech",
            baitUrl: "13032-green-leech",
            coords: '<a data-open="showMap" data-x-coord="[129.464,129.401,129.401,129.401,129.401]" data-y-coord="[651.468,651.407,651.407,651.407,651.407]" data-mapimage="S010S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 129, Y: 651)</sup></a>',
            type: 5,
            startDays: [5],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/oblitus-wood.jpg"
        }, {
            eventName: "Eternal Glacier",
            eventUrl: "55109-eternal-glacier",
            baitName: "Princess Hardscale Fish",
            baitUrl: "14462-princess-hardscale-fish",
            coords: '<a data-open="showMap" data-x-coord="[328.504,328.544,328.557,328.607,328.607]" data-y-coord="[560.044,560.073,560.06,560.044,560.044]" data-mapimage="S019S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 328, Y: 560)</sup></a>',
            type: 5,
            startDays: [5],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/silent-ice-field.jpg"
        }, {
            eventName: "Dark Crystal",
            eventUrl: "55016-dark-crystal",
            baitName: "Forked-Tail Loach",
            baitUrl: "13026-forked-tail-loach",
            coords: '<a data-open="showMap" data-x-coord="[740.342,740.314,740.38,740.314,740.314]" data-y-coord="[595.432,595.485,595.501,595.485,595.485]" data-mapimage="S006S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 740, Y: 595)</sup></a>',
            type: 5,
            startDays: [6],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/demarech-mines.jpg"
        }, {
            eventName: "Crimson Aquadevil",
            eventUrl: "55120-crimson-aquadevil",
            baitName: "Silvercrown Empress Fish",
            baitUrl: "15495-silvercrown-empress-fish",
            coords: '<a data-open="showMap" data-x-coord="[124.0,124.0,124.0,124.0,124.0]" data-y-coord="[175.0,175.0,175.0,175.0,175.0]" data-mapimage="S023S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 124, Y: 175)</sup></a>',
            type: 5,
            startDays: [6],
            startTimes: [3600, 32400, 61200],
            endTimes: [5400, 34200, 63e3],
            img: imgPath + "/images/timers/viridian-steppe.jpg"
        }, {
            eventName: "Sand-Spitting Hermit",
            eventUrl: "55021-sand-spitting-hermit",
            baitName: "Lively Grass Carp",
            baitUrl: "13033-lively-grass-carp",
            coords: '<a data-open="showMap" data-x-coord="[136.848,136.848,136.827,136.776,136.848]" data-y-coord="[790.327,790.327,790.351,790.303,790.327]" data-mapimage="S011S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 136, Y: 790)</sup></a>',
            type: 5,
            startDays: [6],
            startTimes: [18e3, 46800, 75600],
            endTimes: [19800, 48600, 77400],
            img: imgPath + "/images/timers/star-sand-desert.jpg"
        }]
    }, {
        name: "World Bosses",
        events: [{
            eventName: "Hermit Mordecai",
            eventUrl: "41381-hermit-mordecai",
            zoneName: "Crescent Hill",
            zoneUrl: "4-crescent-hill",
            coords: '<a data-open="showMap" data-x-coord="[522.013]" data-y-coord="[793.221]" data-mapimage="S004S" data-map-x-coord="1024" data-map-y-coord="1024" href="javascript:;"><sup>(X: 522, Y: 793)</sup></a>',
            type: 4,
            startDays: [0],
            startTimes: [0, 14400, 28800, 43200, 57600, 72e3],
            endTimes: [600, 15e3, 29400, 43800, 58200, 72600],
            img: imgPath + "/images/timers/crescent-hill.jpg"
        }, {
            eventName: "Pirate Leader Mac",
            eventUrl: "41379-pirate-leader-mac",
            zoneName: "Helonia Coast",
            zoneUrl: "3-helonia-coast",
            coords: '<a data-open="showMap" data-x-coord="[461.669]" data-y-coord="[648.214]" data-mapimage="S003S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 461, Y: 648)</sup></a>',
            type: 4,
            startDays: [4],
            startTimes: [0, 14400, 28800, 43200, 57600, 72e3],
            endTimes: [600, 15e3, 29400, 43800, 58200, 72600],
            img: imgPath + "/images/timers/helonia-coast.jpg"
        }, {
            eventName: "Jungle Panther Banksy",
            eventUrl: "41257-jungle-panther-banksy",
            zoneName: "Candeo Marsh",
            zoneUrl: "8-candeo-marsh",
            coords: '<a data-open="showMap" data-x-coord="[674.886]" data-y-coord="[628.033]" data-mapimage="S008S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 674, Y: 628)</sup></a>',
            type: 4,
            startDays: [5],
            startTimes: [0, 14400, 28800, 43200, 57600, 72e3],
            endTimes: [600, 15e3, 29400, 43800, 58200, 72600],
            img: imgPath + "/images/timers/candeo-marsh.jpg"
        }, {
            eventName: "Secluded Hunter Daphne",
            eventUrl: "41380-secluded-hunter-daphne",
            zoneName: "Triatio Highlands",
            zoneUrl: "7-triatio-highlands",
            coords: '<a data-open="showMap" data-x-coord="[466.31]" data-y-coord="[308.489]" data-mapimage="S007S" data-map-x-coord="1024" data-map-y-coord="1024" href="javascript:;"><sup>(X: 466, Y: 308)</sup></a>',
            type: 4,
            startDays: [6],
            startTimes: [0, 14400, 28800, 43200, 57600, 72e3],
            endTimes: [600, 15e3, 29400, 43800, 58200, 72600],
            img: imgPath + "/images/timers/triatio-highlands.jpg"
        }, {
            eventName: "Colossal Cult Leader Abenthy",
            eventUrl: "44976-colossal-cult-leader-abenthy",
            zoneName: "Desolate Valley",
            zoneUrl: "24-desolate-valley",
            coords: '<a data-open="showMap" data-x-coord="[148.0]" data-y-coord="[589.0]" data-mapimage="S024S" data-map-x-coord="896" data-map-y-coord="896" href="javascript:;"><sup>(X: 148, Y: 589)</sup></a>',
            type: 4,
            startDays: [0],
            startTimes: [7200, 21600, 36e3, 50400, 64800, 79200],
            endTimes: [7800, 22200, 36600, 51e3, 65400, 79800],
            img: imgPath + "/images/timers/desolate-valley.jpg"
        }, {
            eventName: "Enlarged Lavalord Elbiolo",
            eventUrl: "44973-enlarged-lavalord-elbiolo",
            zoneName: "Port Morton",
            zoneUrl: "21-port-morton",
            coords: '<a data-open="showMap" data-x-coord="[652.0]" data-y-coord="[138.0]" data-mapimage="S021S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 652, Y: 138)</sup></a>',
            type: 4,
            startDays: [3],
            startTimes: [7200, 21600, 36e3, 50400, 64800, 79200],
            endTimes: [7800, 22200, 36600, 51e3, 65400, 79800],
            img: imgPath + "/images/timers/port-morton.jpg"
        }, {
            eventName: "Dominating Duskfall Dancer Mechium",
            eventUrl: "44974-dominating-duskfall-dancer-mechium",
            zoneName: "Candetonn Hill",
            zoneUrl: "22-candetonn-hill",
            coords: '<a data-open="showMap" data-x-coord="[544.0]" data-y-coord="[657.0]" data-mapimage="S022S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 544, Y: 657)</sup></a>',
            type: 4,
            startDays: [5],
            startTimes: [7200, 21600, 36e3, 50400, 64800, 79200],
            endTimes: [7800, 22200, 36600, 51e3, 65400, 79800],
            img: imgPath + "/images/timers/candetonn-hill.jpg"
        }, {
            eventName: "Enormous Eclipse Rabbit King Kalishia",
            eventUrl: "44975-enormous-eclipse-rabbit-king-kalishia",
            zoneName: "Viridian Steppe",
            zoneUrl: "23-viridian-steppe",
            coords: '<a data-open="showMap" data-x-coord="[230.0]" data-y-coord="[301.0]" data-mapimage="S023S" data-map-x-coord="832" data-map-y-coord="832" href="javascript:;"><sup>(X: 230, Y: 301)</sup></a>',
            type: 4,
            startDays: [6],
            startTimes: [7200, 21600, 36e3, 50400, 64800, 79200],
            endTimes: [7800, 22200, 36600, 51e3, 65400, 79800],
            img: imgPath + "/images/timers/viridian-steppe.jpg"
        }]
    }, {
        name: "Guild Bosses",
        events: [{
            eventName: "Stormbringer Vayu",
            eventUrl: "61012-stormbringer-vayu",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [0],
            startTimes: [3600, 54e3, 72e3],
            endTimes: [4200, 54600, 72600],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Duke of Darkness Eligos",
            eventUrl: "61008-duke-of-darkness-eligos",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [0],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Nine Tailed Vixen Kotonoha",
            eventUrl: "61016-nine-tailed-vixen-kotonoha",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [1],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Miasmic Serpent Quelkulan",
            eventUrl: "61013-miasmic-serpent-quelkulan",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [2],
            startTimes: [3600, 54e3, 72e3],
            endTimes: [4200, 54600, 72600],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Knight of the Sun Aelius",
            eventUrl: "61007-knight-of-the-sun-aelius",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [2],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Empress of Torment Bel-Chandra",
            eventUrl: "61010-empress-of-torment-bel-chandra",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [3],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Emerald Tempest Yarnaros",
            eventUrl: "61014-emerald-tempest-yarnaros",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [4],
            startTimes: [3600, 54e3, 72e3],
            endTimes: [4200, 54600, 72600],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Iron Titan Gigas",
            eventUrl: "61006-iron-titan-gigas",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [4],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Trampling Thunder Bahadur",
            eventUrl: "61015-trampling-thunder-bahadur",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [5],
            startTimes: [3600, 54e3, 72e3],
            endTimes: [4200, 54600, 72600],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Champion of the Slain Sigrun",
            eventUrl: "61009-champion-of-the-slain-sigrun",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [5],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Feline Emperor Tigerius Caesar",
            eventUrl: "61017-feline-emperor-tigerius-caesar",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [6],
            startTimes: [3600, 54e3, 72e3],
            endTimes: [4200, 54600, 72600],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }, {
            eventName: "Heavenly Defender Uzuriel",
            eventUrl: "61011-heavenly-defender-uzuriel",
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [6],
            startTimes: [43200, 61200, 79200],
            endTimes: [43800, 61800, 79800],
            img: imgPath + "/images/timers/guild-hall.jpg"
        }]
    }, {
        name: "Battlefields",
        events: [{
            eventName: "The Glorious Frost Crown",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [3600, 42e3],
            endTimes: [5400, 43800],
            img: imgPath + "/images/timers/the-glorius-frost-crown.jpg"
        }, {
            eventName: "Tanuki Turmoil",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [10800, 32400, 46800, 77400],
            endTimes: [12600, 34200, 48600, 79200],
            img: imgPath + "/images/timers/tanuki-turmoil.jpg"
        }, {
            eventName: "Excelsior Arena (5 v. 5)",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [18e3, 25200, 37200, 66e3, 84e3],
            endTimes: [19800, 27e3, 39e3, 67800, 85800],
            img: imgPath + "/images/timers/excelsior-arena-5-v-5.jpg"
        }, {
            eventName: "Excelsior Arena (5 v. 5)",
            type: 2,
            startDays: [1, 2, 3],
            startTimes: [53400],
            endTimes: [55200],
            img: imgPath + "/images/timers/excelsior-arena-5-v-5.jpg"
        }, {
            eventName: "Excelsior Arena (5 v. 5)",
            type: 2,
            startDays: [0, 5, 6],
            startTimes: [55200],
            endTimes: [57e3],
            img: imgPath + "/images/timers/excelsior-arena-5-v-5.jpg"
        }, {
            eventName: "Excelsior Arena (5 v. 5)",
            type: 2,
            startDays: [0, 4, 5, 6],
            startTimes: [73200],
            endTimes: [75e3],
            img: imgPath + "/images/timers/excelsior-arena-5-v-5.jpg"
        }, {
            eventName: "Valley of Glorious Battle",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [63e3],
            endTimes: [63600],
            img: imgPath + "/images/timers/valley-of-glorious-battle.jpg"
        }, {
            eventName: "Centurion Battlefield",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [58800],
            endTimes: [60600],
            img: imgPath + "/images/timers/centurion-battlefield.jpg"
        }, {
            eventName: "Centurion Battlefield",
            type: 2,
            startDays: [1, 2, 3],
            startTimes: [73200],
            endTimes: [75e3],
            img: imgPath + "/images/timers/centurion-battlefield.jpg"
        }]
    }, {
        name: "Dungeon Resets",
        events: [{
            eventName: "24 Hour Dungeon",
            type: 3,
            startTimes: [21600],
            endTimes: [21720],
            img: imgPath + "/images/timers/24-hour-dungeon.jpg",
            tooltip: "List of affected dungeons:<hr>Duelists' Temple<br>Gaia's Sanctuary (5 Players)<br>Holy Land of Gaia (Solo)<br>Hall of Philae<br>Gaia's Sanctuary - Trial (Party)<br>Gaia's Sanctuary - Trial (Solo)<br>Vault of Eternity"
        }, {
            eventName: "8 Hour Dungeon",
            type: 3,
            startTimes: [21600, 50400, 79200],
            endTimes: [21720, 50520, 79320],
            img: imgPath + "/images/timers/8-hour-dungeon.jpg",
            tooltip: "List of affected dungeons:<hr>Infernal Abyss<br>Whirlpool Abyss<br>Avarice Abyss"
        }, {
            eventName: "6 Hour Dungeon",
            type: 3,
            startTimes: [0, 21600, 43200, 64800],
            endTimes: [120, 21720, 43320, 64920],
            img: imgPath + "/images/timers/6-hour-dungeon.jpg",
            tooltip: "List of affected dungeons:<hr>Temple of the Eidolons (Solo)<br>Lament of The Thunder-Dragon King<br>Landing of the Sky Dragon King<br>Siege of the Aqua-Dragon Queen<br>Pyroclastic Purgatory<br>Tempestuous Temple<br>Thunder Temple of the Underworld Baroness<br>Frozen Ruins of Zahr-Kazaal"
        }, {
            eventName: "3 Hour Dungeon",
            type: 3,
            startTimes: [0, 10800, 21600, 32400, 43200, 54e3, 64800, 75600],
            endTimes: [120, 10920, 21720, 32520, 43320, 54120, 64920, 75720],
            img: imgPath + "/images/timers/3-hour-dungeon.jpg",
            tooltip: "List of affected dungeons:<hr>All Dungeons (Solo Challenge)<br>Otherworld: Field (All)<br>Otherworld: Dungeon (Hell)"
        }, {
            eventName: "2 Hour Dungeon",
            type: 3,
            startTimes: [0, 7200, 14400, 21600, 28800, 36e3, 43200, 50400, 57600, 64800, 72e3, 79200],
            endTimes: [120, 7320, 14520, 21720, 28920, 36120, 43320, 50520, 57720, 64920, 72120, 79320],
            img: imgPath + "/images/timers/2-hour-dungeon.jpg",
            tooltip: "List of affected dungeons:<hr>Main Dungeon (Solo/Party/Hell)<br>Otherworld: Dungeon (Solo/Party)"
        }]
    }, {
        name: "Raids",
        events: [{
            eventName: "Sky Tower Group 1-30",
            type: 2,
            startDays: [0, 6],
            startTimes: [36e3],
            endTimes: [43200],
            img: imgPath + "/images/timers/sky-tower.jpg"
        }, {
            eventName: "Sky Tower Group 31-60",
            type: 2,
            startDays: [0, 6],
            startTimes: [54e3],
            endTimes: [61200],
            img: imgPath + "/images/timers/sky-tower.jpg"
        }, {
            eventName: "Sky Tower Group 61-90",
            type: 2,
            startDays: [0, 6],
            startTimes: [72e3],
            endTimes: [79200],
            img: imgPath + "/images/timers/sky-tower.jpg"
        }, {
            eventName: "Sky Tower Elite Group 1-30",
            type: 2,
            startDays: [5],
            startTimes: [36e3],
            endTimes: [43200],
            img: imgPath + "/images/timers/sky-tower-elite.jpg"
        }, {
            eventName: "Sky Tower Elite Group 31-60",
            type: 2,
            startDays: [5],
            startTimes: [54e3],
            endTimes: [61200],
            img: imgPath + "/images/timers/sky-tower-elite.jpg"
        }, {
            eventName: "Sky Tower Elite Group 61-90",
            type: 2,
            startDays: [5],
            startTimes: [72e3],
            endTimes: [79200],
            img: imgPath + "/images/timers/sky-tower-elite.jpg"
        }, {
            eventName: "Sky Realm Group 1-5",
            type: 2,
            startDays: [4],
            startTimes: [36e3],
            endTimes: [39e3],
            img: imgPath + "/images/timers/sky-realm.jpg"
        }, {
            eventName: "Sky Realm Group 6-10",
            type: 2,
            startDays: [4],
            startTimes: [54e3],
            endTimes: [57e3],
            img: imgPath + "/images/timers/sky-realm.jpg"
        }, {
            eventName: "Sky Realm Group 11-15",
            type: 2,
            startDays: [4],
            startTimes: [72e3],
            endTimes: [75e3],
            img: imgPath + "/images/timers/sky-realm.jpg"
        }]
    }, {
        name: "Aura Kingdom Quiz",
        events: [{
            eventName: "Aura Kingdom Quiz",
            type: 2,
            startDays: [0, 1, 2, 3, 4, 5, 6],
            startTimes: [29700, 51300, 70500, 81600],
            endTimes: [30600, 52200, 71400, 82500],
            img: imgPath + "/images/timers/aura-kingdom-quiz.jpg"
        }]
    }, {
        name: "Card Rangers",
        events: [{
            eventName: "All Rangers",
            eventUrl: null,
            zoneName: null,
            zoneUrl: null,
            coords: null,
            type: 4,
            startDays: [0, 6],
            startTimes: [61200, 75600],
            endTimes: [68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }, {
            eventName: "Card Rangers Red Ranger",
            eventUrl: "11539-card-rangers-red-ranger",
            zoneName: null,
            zoneUrl: null,
            coords: '<a data-open="showMap" data-x-coord="[374.781,374.941,381.099,381.184,371.666,372.646,371.75,372.685]" data-y-coord="[735.949,737.942,740.221,742.557,748.599,748.823,750.051,750.004]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 374, Y: 735)</sup></a>',
            type: 4,
            startDays: [1],
            startTimes: [3600, 43200, 61200, 75600],
            endTimes: [10800, 50400, 68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }, {
            eventName: "Card Rangers Black Ranger",
            eventUrl: "11540-card-rangers-black-ranger",
            zoneName: null,
            zoneUrl: null,
            coords: '<a data-open="showMap" data-x-coord="[384.683,384.722,389.72,389.614,375.535,377.025,375.604,376.958]" data-y-coord="[735.333,737.813,740.085,743.157,747.755,747.717,749.209,749.129]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 384, Y: 735)</sup></a>',
            type: 4,
            startDays: [2],
            startTimes: [3600, 43200, 61200, 75600],
            endTimes: [10800, 50400, 68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }, {
            eventName: "Card Rangers Blue Ranger",
            eventUrl: "11541-card-rangers-blue-ranger",
            zoneName: null,
            zoneUrl: null,
            coords: '<a data-open="showMap" data-x-coord="[382.193,382.29,387.401,387.413,372.93,374.267,374.27,372.822]" data-y-coord="[735.451,737.895,740.42,742.059,760.777,760.803,759.496,759.493]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 382, Y: 735)</sup></a>',
            type: 4,
            startDays: [3],
            startTimes: [3600, 43200, 61200, 75600],
            endTimes: [10800, 50400, 68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }, {
            eventName: "Card Rangers Pink Ranger",
            eventUrl: "11543-card-rangers-pink-ranger",
            zoneName: null,
            zoneUrl: null,
            coords: '<a data-open="showMap" data-x-coord="[379.591,379.657,385.322,385.089,379.137,378.326,377.12,377.753]" data-y-coord="[735.782,737.954,740.3,742.631,760.911,759.622,759.613,760.775]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 379, Y: 735)</sup></a>',
            type: 4,
            startDays: [4],
            startTimes: [3600, 43200, 61200, 75600],
            endTimes: [10800, 50400, 68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }, {
            eventName: "Card Rangers Yellow Ranger",
            eventUrl: "11542-card-rangers-yellow-ranger",
            zoneName: null,
            zoneUrl: null,
            coords: '<a data-open="showMap" data-x-coord="[377.158,377.219,383.425,383.298,384.604,384.541,382.515,382.603]" data-y-coord="[735.838,737.786,739.99,742.644,747.505,749.108,747.619,749.079]" data-mapimage="S009S" data-map-x-coord="960" data-map-y-coord="960" href="javascript:;"><sup>(X: 377, Y: 735)</sup></a>',
            type: 4,
            startDays: [5],
            startTimes: [3600, 43200, 61200, 75600],
            endTimes: [10800, 50400, 68400, 82800],
            img: imgPath + "/images/timers/card-rangers.jpg"
        }]
    }];