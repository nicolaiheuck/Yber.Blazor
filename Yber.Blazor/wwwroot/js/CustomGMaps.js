let map;

async function initMap(listOfLocations, currentLocation, encodedPolyline) {
    // The location of EUC Syd
    const position = { lat: 54.909290, lng: 9.799820 };


    // TODO Add the current user location
    // Use currentLocation
    const originPos = { lat: 54.90567, lng: 9.7974 }
    
    // Import from Google API
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    const { PinElement } = await google.maps.importLibrary("marker");
    const { encoding } = await google.maps.importLibrary("geometry");

    // The map, centered at EUC
    map = new Map(document.getElementById("map"), {
        zoom: 13,
        mapId: "d808d2842cb1ee99",
        center: position,
        disableDefaultUI: true,
        mapTypeControl: false,
    });
    
    // TODO Add the encoded polyline
    // Use encodedPolyline
    var decodedPath =  encoding.decodePath("yubnIwkxz@iAjD[t@a@@kAHm@Ds@JIC]YYYeChKSd@QTqC}Bq@w@wByCaEyF^oCJ[FIfBsED_@j@}Bj@}CTV");
    var decodedLevels = decodeLevels("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
    
    polyLine = new google.maps.Polyline({
        path: decodedPath,
        levels: decodedLevels,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });
    
    polyLine.setMap(map);

    // The marker, positioned at EUC
    const eucSyd = new AdvancedMarkerElement({
        map: map,
        position: position,
        title: "EUC Syd",
    });
    
    // Marker, positioned at current location
    const origin = new AdvancedMarkerElement({
        map: map,
        position: originPos,
        title: "Dig"
    });
    
    // listOfLocations
    var debug = [{lat: 54.911140, lng: 9.798410}, {lat: 54.912340, lng: 9.798410}, {lat: 54.913640, lng: 9.798410}];
    addMarkers(debug);
    // TODO Add the list of users from the API!
    // Use listOfLocations 
    
    function addMarkers(latLngArray) {
        const pinColorNeedLift = new PinElement({
            background: "#00FF00",
        });
        
        for (i = 0; i < latLngArray.length; i++) {

            var marker = new AdvancedMarkerElement({
                map: map,
                position: latLngArray[i],
                content: pinColor.element
            })
        }
    }
}

function decodeLevels(encodedLevelsString) {
    var decodedLevels = [];

    for (var i = 0; i < encodedLevelsString.length; ++i) {
        var level = encodedLevelsString.charCodeAt(i) - 63;
        decodedLevels.push(level);
    }
    return decodedLevels;
}


initMap();