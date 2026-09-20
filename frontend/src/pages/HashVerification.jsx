import { useState } from "react";

function HashVerification() {
    const [input, setInput] = useState("");
    const [algorithm, setAlgorithm] = useState("SHA256");
    const [expectedHash, setExpectedHash] = useState("");
    const [result, setResult] = useState(null);
    const [error, setError] = useState("");

    async function handleVerify(event) {
        event.preventDefault();

        setResult(null);
        setError("");

        try {
            const response = await fetch(
                "http://localhost:5131/api/HashVerification/verify",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        input: input,
                        algorithm: algorithm,
                        expectedHash: expectedHash
                    })
                }
            );

            const data = await response.json();

            if (!response.ok) {
                throw new Error(
                    data.message ||
                    "Failed to verify hash."
                );
            }

            setResult(data);

        } catch (error) {
            console.error(
                "Hash Verification error:",
                error
            );

            setError(error.message);
        }
    }

    async function handleCopy() {
        if (!result?.generatedHash) {
            return;
        }

        try {
            await navigator.clipboard.writeText(
                result.generatedHash
            );

        } catch (error) {
            console.error(
                "Copy hash error:",
                error
            );

            setError("Failed to copy hash.");
        }
    }

    return (
        <div>
            <h1>Hash Verification</h1>

            <p>
                Verify whether a hash matches the
                supplied input.
            </p>

            <form onSubmit={handleVerify}>

                <div>
                    <label htmlFor="input">
                        Original Input:
                    </label>

                    <textarea
                        id="input"
                        value={input}
                        onChange={(event) =>
                            setInput(event.target.value)
                        }
                        placeholder="Enter the original text"
                        required
                    />
                </div>

                <div>
                    <label htmlFor="algorithm">
                        Hash Algorithm:
                    </label>

                    <select
                        id="algorithm"
                        value={algorithm}
                        onChange={(event) =>
                            setAlgorithm(event.target.value)
                        }
                    >
                        <option value="SHA256">
                            SHA-256
                        </option>

                        <option value="SHA512">
                            SHA-512
                        </option>
                    </select>
                </div>

                <div>
                    <label htmlFor="expectedHash">
                        Expected Hash:
                    </label>

                    <textarea
                        id="expectedHash"
                        value={expectedHash}
                        onChange={(event) =>
                            setExpectedHash(event.target.value)
                        }
                        placeholder="Enter the hash to verify"
                        required
                    />
                </div>

                <button type="submit">
                    Verify Hash
                </button>

            </form>

            {error && (
                <p>
                    {error}
                </p>
            )}

            {result && (
                <div>
                    <h2>Verification Result</h2>

                    <p>
                        <strong>Algorithm:</strong>{" "}
                        {result.algorithm}
                    </p>

                    <p>
                        <strong>Generated Hash:</strong>
                    </p>

                    <p>
                        {result.generatedHash}
                    </p>

                    <button
                        type="button"
                        onClick={handleCopy}
                    >
                        Copy Generated Hash
                    </button>

                    <p>
                        <strong>Result:</strong>{" "}
                        {result.isMatch
                            ? "Hash matches"
                            : "Hash does not match"}
                    </p>
                </div>
            )}
        </div>
    );
}

export default HashVerification;