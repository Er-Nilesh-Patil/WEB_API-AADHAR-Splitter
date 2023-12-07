async function findAadhaarNumbers() {
    try {
        console.log("Start findAadhaarNumbers");

        // Get raw text from the textarea
        var rawText = document.getElementById("rawTextInput").value;

        // Make a request to the API
        const response = await fetch("/api/Aadhaar/FindAadhaar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(rawText),
        });

        console.log("After fetch");

        // Check if the response status is OK (200)
        if (response.ok) {
            const data = await response.json();

            console.log("Data received:", data);

            // Update the listbox with the extracted Aadhaar numbers
            var aadhaarList = document.getElementById("aadhaarList");
            aadhaarList.innerHTML = "";

            if (data.length > 0) {
                data.forEach(aadhaarNumber => {
                    var option = document.createElement("option");
                    option.text = aadhaarNumber;
                    aadhaarList.add(option);
                });
            } else {
                // Handle the case when no Aadhaar numbers are found
                console.log("No Aadhaar numbers found.");
            }
        } else {
            // Handle non-OK responses (e.g., 404 Not Found)
            throw new Error(`Request failed with status: ${response.status}`);
        }
    } catch (error) {
        console.error("Error:", error);
    }
}
