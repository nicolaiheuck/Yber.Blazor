let map;

async function initMap(listOfLocations, currentLocation, encodedPolyline) {
    // The location of EUC Syd
    const position = { lat: 54.909290, lng: 9.799820 };
    let jsonOriginPos = JSON.parse(currentLocation);
    const originPos = jsonOriginPos.latlng;
    
    // Import libs from Google
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    const { PinElement } = await google.maps.importLibrary("marker");
    const { encoding } = await google.maps.importLibrary("geometry");

    // The custom map, centered at EUC
    map = new Map(document.getElementById("map"), {
        zoom: 13,
        mapId: "d808d2842cb1ee99",
        center: position,
        disableDefaultUI: true,
        mapTypeControl: false,
    });
    
    var decodedPath =  encoding.decodePath(encodedPolyline);
    
    polyLine = new google.maps.Polyline({
        path: decodedPath,
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
    
    addMarkers(JSON.parse(listOfLocations));
    
    function addMarkers(jsonStudentDTO) {
        for (i = 0; i < jsonStudentDTO.length; i++) {
            var marker = new AdvancedMarkerElement({
                map: map,
                position: jsonStudentDTO[i]["latlng"],
                title: jsonStudentDTO[i]["name"],
                content: new PinElement({
                    background: "#00FF00",
                }).element
            })
        }
    }
}

initMap();