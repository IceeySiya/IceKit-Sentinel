import { useState } from "react";

function SecurityHeaders() {
    const [url, setUrl] = useState("");
    const [result, setResult] = useState(null);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const analyzeHeaders = async (event) => {
        event.preventDefault();

        setError("");
        setResult(null);
        setLoading(true);

        try {
            const response = await fetch(
                "http://localhost:5131/api/SecurityHeaders/analyze",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        url: url
                    })
                }
            );

            const data = await response.json();

            if (!response.ok) {
                throw new Error(
                    data.message || "Unable to analyze the URL."
                );
            }

            setResult(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h1>Security Headers Checker</h1>

            <p>
                Analyze a website's HTTP response headers for
                commonly recommended security headers.
            </p>

            <form onSubmit={analyzeHeaders}>
                <input
                    type="url"
                    placeholder="https://example.com"
                    value={url}
                    onChange={(event) => setUrl(event.target.value)}
                    required
                />

                <button
                    type="submit"
                    disabled={loading}
                >
                    {loading ? "Analyzing..." : "Analyze Headers"}
                </button>
            </form>

            {error && (
                <div>
                    <p>{error}</p>
                </div>
            )}

            {result && (
                <div>
                    <h2>Analysis Results</h2>

                    <p>
                        <strong>URL:</strong> {result.url}
                    </p>

                    <p>
                        <strong>Status Code:</strong>{" "}
                        {result.statusCode}
                    </p>

                    <p>
                        <strong>HTTPS:</strong>{" "}
                        {result.isHttps ? "Yes" : "No"}
                    </p>

                    <p>
                        <strong>Security Headers:</strong>{" "}
                        {result.headersPresent} /{" "}
                        {result.headersChecked}
                    </p>

                    <h3>Header Results</h3>

                    <table>
                        <thead>
                            <tr>
                                <th>Security Header</th>
                                <th>Present</th>
                            </tr>
                        </thead>

                        <tbody>
                            {Object.entries(
                                result.securityHeaders
                            ).map(([header, present]) => (
                                <tr key={header}>
                                    <td>{header}</td>
                                    <td>
                                        {present
                                            ? "Present"
                                            : "Missing"}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default SecurityHeaders;