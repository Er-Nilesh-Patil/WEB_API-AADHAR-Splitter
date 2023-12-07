function findAadhaarNumbers() {
    
    var rawText = document.getElementById("rawTextInput").value;

  
    fetch("/api/Aadhaar/FindAadhaar", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(rawText),
    })
        .then(response => response.json())
        .then(data => {
           
            var aadhaarList = document.getElementById("aadhaarList");
            aadhaarList.innerHTML = "";

            data.forEach(aadhaarNumber => {
                var option = document.createElement("option");
                option.text = aadhaarNumber;
                aadhaarList.add(option);
            });
        })
        .catch(error => {
            console.error("Error:", error);
        });
}




/*// Ensure the script.js file starts with a valid comment or statement

function findAadhaarNumbers() {
    // Get raw text from the textarea
    var rawText = document.getElementById("rawTextInput").value;

    // Make a request to the API
    fetch("/api/Aadhaar/FindAadhaar", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(rawText),
    })
        .then(response => {
            // Check if the response status is OK (200)
            if (response.ok) {
                return response.json();
            } else {
                // Handle non-OK responses (e.g., 404 Not Found)
                throw new Error(`Request failed with status: ${response.status}`);
            }
        })
        .then(data => {
            // Update the listbox with the extracted Aadhaar numbers
            var aadhaarList = document.getElementById("aadhaarList");
            aadhaarList.innerHTML = "";

            data.forEach(aadhaarNumber => {
                var option = document.createElement("option");
                option.text = aadhaarNumber;
                aadhaarList.add(option);
            });
        })
        .catch(error => {
            console.error("Error:", error);
        });
}
*/