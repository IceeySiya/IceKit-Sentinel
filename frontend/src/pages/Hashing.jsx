import { useState } from "react";

function Hashing() {
    const [input, setInput] = useState("");
    const [algorithm, setAlgorithm] = useState("SHA256");
    const [result, setResult] = useState(null);
    const [error, setError] = useState("");
    const [copied, setCopied] = useState(false);

    async function handleGenerateHash(event) {
        event.preventDefault();

        setResult(null);
        setError("");
        setCopied(false);

        try {
            const response = await fetch(
                "http://localhost:5131/api/Hash/generate",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        input: input,
                        algorithm: algorithm
                    })
                }
            );

            const data = await response.json();

            if (!response.ok) {
                throw new Error(
                    data.message || "Failed to generate hash."
                );
            }

            setResult(data);

        } catch (error) {
            console.error(
                "Hashing error:",
                error
            );

            setError(error.message);
        }
    }

    async function handleCopy() {
        if (!result?.hash) {
            return;
        }

        try {
            await navigator.clipboard.writeText(
                result.hash
            );

            setCopied(true);

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
            <h1>Hash Generator</h1>

            <p>
                Generate a SHA-256 or SHA-512 hash
                from input text.
            </p>

            <form onSubmit={handleGenerateHash}>

                <div>
                    <label htmlFor="input">
                        Input:
                    </label>

                    <textarea
                        id="input"
                        value={input}
                        onChange={(event) =>
                            setInput(event.target.value)
                        }
                        placeholder="Enter text to hash"
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

                <button type="submit">
                    Generate Hash
                </button>

            </form>

            {error && (
                <p>
                    {error}
                </p>
            )}

            {result && (
                <div>
                    <h2>Hash Result</h2>

                    <p>
                        <strong>Algorithm:</strong>{" "}
                        {result.algorithm}
                    </p>

                    <p>
                        <strong>Input Length:</strong>{" "}
                        {result.inputLength}
                    </p>

                    <h3>Hash</h3>

                    <p>
                        {result.hash}
                    </p>

                    <button
                        type="button"
                        onClick={handleCopy}
                    >
                        Copy Hash
                    </button>

                    {copied && (
                        <p>
                            Hash copied to clipboard.
                        </p>
                    )}
                </div>
            )}
        </div>
    );
}

export default Hashing;