let html5QrcodeScanner = new Html5QrcodeScanner(
    "reader",
    {
        fps: 10,
        qrbox:
        {
            width: 50
            , height: 50
        }
    }, false);


function onScanSuccess(decodedText, decodedResult) {
    console.log(`Code matched = ${decodedText}`, decodedResult);
    window.location.href = "ScanResult?Code=" + decodedText;
    html5QrcodeScanner.clear();
}

function onScanFailure(error) {
    console.error(`Code scan error = ${error}`);
}
html5QrcodeScanner.render(onScanSuccess, onScanFailure);