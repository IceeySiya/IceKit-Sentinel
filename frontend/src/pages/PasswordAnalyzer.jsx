import { useState } from "react";

function PasswordAnalyzer() {

    const [password, setPassword] = useState("");
    const [result, setResult] = useState(null);
    const [error, setError] = useState("");

    async function handleAnalyze(event) {
        event.preventDefault();

        setResult(null);
        setError("");

        try {
            const response = await fetch(
                "http://localhost:5131/api/PasswordAnalysis/analyze",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        password: password
                    })
                }
            );

            const data = await response.json();

            if (!response.ok) {
                throw new Error(
                    data.message || "Failed to analyze password."
                );
            }

            setResult(data);

        } catch (error) {
            setError(error.message);
        }
    }

    return (
        <div>

            <h1>Password Security Analyzer</h1>

            <p>
                Analyze the security characteristics of a password.
            </p>

            <form onSubmit={handleAnalyze}>

                <div>
                    <label htmlFor="password">
                        Password:
                    </label>

                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(event) =>
                            setPassword(event.target.value)
                        }
                        required
                    />
                </div>

                <button type="submit">
                    Analyze Password
                </button>

            </form>

            {error && (
                <p>
                    {error}
                </p>
            )}

            {result && (
                <div>

                    <h2>Analysis Result</h2>

                    <p>
                        <strong>Length:</strong>{" "}
                        {result.length}
                    </p>

                    <p>
                        <strong>Score:</strong>{" "}
                        {result.score} / 5
                    </p>

                    <p>
                        <strong>Strength:</strong>{" "}
                        {result.strength}
                    </p>

                    <h3>Character Types</h3>

                    <p>
                        Lowercase:{" "}
                        {result.hasLowercase ? "Yes" : "No"}
                    </p>

                    <p>
                        Uppercase:{" "}
                        {result.hasUppercase ? "Yes" : "No"}
                    </p>

                    <p>
                        Numbers:{" "}
                        {result.hasNumbers ? "Yes" : "No"}
                    </p>

                    <p>
                        Special Characters:{" "}
                        {result.hasSpecialCharacters
                            ? "Yes"
                            : "No"}
                    </p>

                    <h3>Recommendations</h3>

                    {result.recommendations &&
                        result.recommendations.map(
                            (recommendation, index) => (
                                <p key={index}>
                                    • {recommendation}
                                </p>
                            )
                        )}

                </div>
            )}

        </div>
    );
}

export default PasswordAnalyzer;