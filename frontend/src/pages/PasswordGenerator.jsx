import { useState } from "react";

function PasswordGenerator() {
    // Stores the options selected by the user.
    const [formData, setFormData] = useState({
        length: 12,
        includeLowercase: true,
        includeUppercase: true,
        includeNumbers: true,
        includeSpecialCharacters: true
    });

    // Stores the password returned by the backend.
    const [result, setResult] = useState(null);

    // Stores an error message if something goes wrong.
    const [error, setError] = useState("");

    // Tracks whether the generated password was copied.
    const [copied, setCopied] = useState(false);

    // Handles changes to the form inputs.
    function handleChange(event) {
        const { name, value, type, checked } = event.target;

        setFormData({
            ...formData,

            // Checkboxes use "checked".
            // The number input uses "value".
            [name]: type === "checkbox"
                ? checked
                : Number(value)
        });
    }

    // Sends the password generation request to the backend.
    async function handleGenerate(event) {
        event.preventDefault();

        // Clear previous results and messages.
        setResult(null);
        setError("");
        setCopied(false);

        try {
            const response = await fetch(
                "http://localhost:5131/api/PasswordGenerator/generate",
                {
                    method: "POST",

                    headers: {
                        "Content-Type": "application/json"
                    },

                    body: JSON.stringify(formData)
                }
            );

            // Convert the JSON response into a JavaScript object.
            const data = await response.json();

            // Check whether the HTTP request was successful.
            if (!response.ok) {
                throw new Error(
                    data.message ||
                    "Failed to generate password."
                );
            }

            // Store the backend response in React state.
            setResult(data);

        } catch (error) {
            console.error(
                "Password Generator error:",
                error
            );

            setError(error.message);
        }
    }

    // Copies the generated password to the clipboard.
    async function handleCopy() {
        // Make sure a password actually exists.
        if (!result?.password) {
            return;
        }

        try {
            await navigator.clipboard.writeText(
                result.password
            );

            setCopied(true);

        } catch (error) {
            console.error(
                "Copy password error:",
                error
            );

            setError("Failed to copy password.");
        }
    }

    return (
        <div>
            <h1>Password Generator</h1>

            <p>
                Generate a secure random password.
            </p>

            <form onSubmit={handleGenerate}>

                {/* Password length */}
                <div>
                    <label htmlFor="length">
                        Password Length:
                    </label>

                    <input
                        type="number"
                        id="length"
                        name="length"
                        min="12"
                        max="128"
                        value={formData.length}
                        onChange={handleChange}
                    />
                </div>

                {/* Lowercase characters */}
                <div>
                    <label>
                        <input
                            type="checkbox"
                            name="includeLowercase"
                            checked={
                                formData.includeLowercase
                            }
                            onChange={handleChange}
                        />

                        Lowercase
                    </label>
                </div>

                {/* Uppercase characters */}
                <div>
                    <label>
                        <input
                            type="checkbox"
                            name="includeUppercase"
                            checked={
                                formData.includeUppercase
                            }
                            onChange={handleChange}
                        />

                        Uppercase
                    </label>
                </div>

                {/* Numbers */}
                <div>
                    <label>
                        <input
                            type="checkbox"
                            name="includeNumbers"
                            checked={
                                formData.includeNumbers
                            }
                            onChange={handleChange}
                        />

                        Numbers
                    </label>
                </div>

                {/* Special characters */}
                <div>
                    <label>
                        <input
                            type="checkbox"
                            name="includeSpecialCharacters"
                            checked={
                                formData.includeSpecialCharacters
                            }
                            onChange={handleChange}
                        />

                        Special Characters
                    </label>
                </div>

                <button type="submit">
                    Generate Password
                </button>

            </form>

            {/* Display an error if something went wrong. */}
            {error && (
                <p>
                    {error}
                </p>
            )}

            {/* Display the generated password and information. */}
            {result && (
                <div>
                    <h2>Generated Password</h2>

                    <p>
                        {result.password}
                    </p>

                    <button
                        type="button"
                        onClick={handleCopy}
                    >
                        Copy Password
                    </button>

                    {copied && (
                        <p>
                            Password copied to clipboard.
                        </p>
                    )}

                    <h3>Password Information</h3>

                    <p>
                        Length: {result.length}
                    </p>

                    <p>
                        Lowercase:{" "}
                        {result.hasLowercase
                            ? "Yes"
                            : "No"}
                    </p>

                    <p>
                        Uppercase:{" "}
                        {result.hasUppercase
                            ? "Yes"
                            : "No"}
                    </p>

                    <p>
                        Numbers:{" "}
                        {result.hasNumbers
                            ? "Yes"
                            : "No"}
                    </p>

                    <p>
                        Special Characters:{" "}
                        {result.hasSpecialCharacters
                            ? "Yes"
                            : "No"}
                    </p>

                </div>
            )}
        </div>
    );
}

export default PasswordGenerator;
