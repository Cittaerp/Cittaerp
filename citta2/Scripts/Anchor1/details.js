
function opendetail(key) {
    var key1=key.substring(1)
    var ind = key.substring(0, 1);
    if (ind == "I")
        key1 = "hcheck/scandisp?anc=" + key1;
    else if (ind == "P")
        key1 = "hcheck/paydisp?anc=" + key1;
    else if (ind == "S")
        key1 = "hcheck/staffdisp?anc=" + key1;
    else if (ind == "A")
        key1 = "pcheck/apprdisp?anc=" + key1;
    else if (ind == "V")
        key1 = "hcheck/tadvdisp?anc=" + key1;
    else if (ind == "C")
        key1 = "hcheck/appldisp?anc=" + key1;
    else if (ind == "M")
        key1 = "hcheck/actdisp?anc=" + key1;
    else if (ind == "Z")
        key1 = "hcheck/tpostdisp?anc=" + key1;
    else if (ind == "F")
        key1 = "hcheck/incidentdisp?anc=" + key1;
    else if (ind == "R")
        key1 = "hcheck/rejectdisp?anc=" + key1.substring(1);    
    else
        key1 = "hcheck/happdisp?anc=" + key1;

    var l3 = $("#locat").val() + key1;
    window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,toolbar=0,resizable=1,scrollbars=1');

    }

function approvedetail(key) {
    var key1 = key.substring(1)
    var ind = key.substring(0, 1);
    if (ind == "I")
        key1 = "hcheck/scanapp?anc=" + key1;
    else if (ind == "P")
        key1 = "hcheck/payapp?anc=" + key1;
    else if (ind == "S")
        key1 = "hcheck/staffapp?anc=" + key1;
    else if (ind == "A")
        key1 = "pcheck/apprapp?anc=" + key1;
    else if (ind == "V")
        key1 = "hcheck/tadvapp?anc=" + key1;
    else if (ind == "Z")
        key1 = "hcheck/tpostapp?anc=" + key1;
    else if (ind == "F")
        key1 = "hcheck/incidentapp?anc=" + key1;
    else
        key1 = "hcheck/happ?anc=" + key1;

    var l3 = $("#locat").val() + key1;    
    window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,toolbar=0,resizable=1,scrollbars=1');

}

function showdocument(key) {
    var key1 = key.substring(1)
    var ind = key.substring(0, 2);
    key1 = "basecall/showdoc?det_str=" + key;

    var l3 = $("#locat").val() + key1;    
    window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,resizable=yes,scrollbars=yes');

}

function mastdetail(key) {
    var key1 = key.substring(1)
    var ind = key.substring(0, 1);
    if (ind == "I")
        key1 = "hcheck/scanapp?anc=" + key1;
    else if (ind == "P")
        key1 = "hcheck/payapp?anc=" + key1;
    else if (ind == "S")
        key1 = "hcheck/staffapp?anc=" + key1;
    else if (ind == "A")
        key1 = "pcheck/apprapp?anc=" + key1;
    else if (ind == "V")
        key1 = "hcheck/madvdisp?anc=" + key1;
    else if (ind == "Z")
        key1 = "hcheck/tpostapp?anc=" + key1;
    else
        key1 = "hcheck/happ?anc=" + key1;

    var l3 = $("#locat").val() + key1;
    window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,toolbar=0,resizable=1,scrollbars=1');

}

function showpara(key) {
    var key1 = key.substring(1)
    var ind = key.substring(0, 1);
    key1 = "hcheck/showdpara?det_str=" + key1;

    var l3 = $("#locat").val() + key1;
    window.open(l3, '_blank', 'left=100,top=50,width=1000,height=650,resizable=yes,scrollbars=yes');

}

function showbox(key) {

    var l3 = $("#locat").val() + key;
    window.open(l3, '_blank', 'left=100,top=50,width=500,height=400,resizable=yes,scrollbars=yes');

}

