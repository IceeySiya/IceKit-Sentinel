import { useEffect, useState } from "react";

function FileIntegrity() {

    // -----------------------------------------
    // REGISTER FILE STATE
    // -----------------------------------------

    // Stores the file selected for registration.
    const [registerFile, setRegisterFile] = useState(null);

    // Stores the hashing algorithm selected for registration.
    const [registerAlgorithm, setRegisterAlgorithm] = useState("SHA256");


    // -----------------------------------------
    // CHECK FILE STATE
    // -----------------------------------------

    // Stores the file selected for integrity checking.
    const [checkFile, setCheckFile] = useState(null);

    // Stores the ID of the selected database fingerprint.
    const [selectedRecordId, setSelectedRecordId] = useState("");


    // -----------------------------------------
    // DATABASE STATE
    // -----------------------------------------

    // Stores all registered file fingerprints.
    const [registeredFiles, setRegisteredFiles] = useState([]);


    // -----------------------------------------
    // RESULT / ERROR STATE
    // -----------------------------------------

    const [registerResult, setRegisterResult] = useState(null);

    const [checkResult, setCheckResult] = useState(null);

    const [error, setError] = useState("");

    const [loading, setLoading] = useState(false);


    // -----------------------------------------
    // LOAD REGISTERED FILES
    // -----------------------------------------

    useEffect(() => {
        loadRegisteredFiles();
    }, []);


    async function loadRegisteredFiles() {

        try {

            setError("");

            const response = await fetch(
                "http://localhost:5131/api/FileIntegrity"
            );

            const data = await response.json();

            if (!response.ok) {

                throw new Error(
                    data.message ||
                    "Failed to load registered files."
                );
            }

            setRegisteredFiles(data);

        } catch (error) {

            console.error(
                "Load registered files error:",
                error
            );

            setError(error.message);
        }
    }


    // -----------------------------------------
    // REGISTER FILE
    // -----------------------------------------

    async function handleRegister(event) {

        event.preventDefault();

        setRegisterResult(null);
        setCheckResult(null);
        setError("");

        if (!registerFile) {

            setError(
                "Please select a file to register."
            );

            return;
        }

        try {

            setLoading(true);

            const formData = new FormData();

            formData.append(
                "File",
                registerFile
            );

            formData.append(
                "Algorithm",
                registerAlgorithm
            );


            const response = await fetch(
                "http://localhost:5131/api/FileIntegrity/register",
                {
                    method: "POST",
                    body: formData
                }
            );


            const data = await response.json();


            if (!response.ok) {

                throw new Error(
                    data.message ||
                    "Failed to register file."
                );
            }


            setRegisterResult(data);

            // Clear the selected registration file.
            setRegisterFile(null);

            // Refresh the fingerprint table.
            await loadRegisteredFiles();

        } catch (error) {

            console.error(
                "Register file error:",
                error
            );

            setError(error.message);

        } finally {

            setLoading(false);
        }
    }


    // -----------------------------------------
    // CHECK FILE INTEGRITY
    // -----------------------------------------

    async function handleCheck(event) {

        event.preventDefault();

        setCheckResult(null);
        setRegisterResult(null);
        setError("");

        if (!checkFile) {

            setError(
                "Please select a file to check."
            );

            return;
        }

        if (!selectedRecordId) {

            setError(
                "Please select a registered file fingerprint."
            );

            return;
        }


        try {

            setLoading(true);

            const formData = new FormData();

            formData.append(
                "File",
                checkFile
            );

            // Send the database fingerprint ID.
            formData.append(
                "FileIntegrityRecordId",
                selectedRecordId
            );


            const response = await fetch(
                "http://localhost:5131/api/FileIntegrity/check",
                {
                    method: "POST",
                    body: formData
                }
            );


            const data = await response.json();


            if (!response.ok) {

                throw new Error(
                    data.message ||
                    "Failed to check file integrity."
                );
            }


            setCheckResult(data);

        } catch (error) {

            console.error(
                "Check file integrity error:",
                error
            );

            setError(error.message);

        } finally {

            setLoading(false);
        }
    }


    // -----------------------------------------
    // FORMAT FILE SIZE
    // -----------------------------------------

    function formatFileSize(bytes) {

        if (bytes < 1024) {
            return `${bytes} B`;
        }

        if (bytes < 1024 * 1024) {
            return `${(bytes / 1024).toFixed(2)} KB`;
        }

        return `${(bytes / (1024 * 1024)).toFixed(2)} MB`;
    }


    // -----------------------------------------
    // PAGE
    // -----------------------------------------

    return (
        <div>

            <h1>File Integrity Checker</h1>

            <p>
                Register files as known-good fingerprints
                and verify whether files have been modified.
            </p>


            {/* -----------------------------------------
                ERROR MESSAGE
            ----------------------------------------- */}

            {error && (
                <div>

                    <p>
                        {error}
                    </p>

                </div>
            )}


            {/* -----------------------------------------
                REGISTER FILE
            ----------------------------------------- */}

            <section>

                <h2>Register File</h2>

                <p>
                    Register a file as a known-good baseline.
                    IceKit will generate its cryptographic hash
                    and store the fingerprint in the database.
                </p>


                <form onSubmit={handleRegister}>

                    <div>

                        <label htmlFor="registerFile">
                            Select File:
                        </label>

                        <input
                            type="file"
                            id="registerFile"
                            onChange={(event) =>
                                setRegisterFile(
                                    event.target.files[0]
                                )
                            }
                        />

                    </div>


                    <div>

                        <label htmlFor="registerAlgorithm">
                            Hash Algorithm:
                        </label>

                        <select
                            id="registerAlgorithm"
                            value={registerAlgorithm}
                            onChange={(event) =>
                                setRegisterAlgorithm(
                                    event.target.value
                                )
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


                    <button
                        type="submit"
                        disabled={loading}
                    >
                        {loading
                            ? "Registering..."
                            : "Register File"}
                    </button>

                </form>


                {/* Registration Result */}

                {registerResult && (

                    <div>

                        <h3>
                            File Registered Successfully
                        </h3>

                        <p>
                            <strong>File Name:</strong>{" "}
                            {registerResult.fileName}
                        </p>

                        <p>
                            <strong>File Size:</strong>{" "}
                            {formatFileSize(
                                registerResult.fileSize
                            )}
                        </p>

                        <p>
                            <strong>Algorithm:</strong>{" "}
                            {registerResult.algorithm}
                        </p>

                        <p>
                            <strong>File Hash:</strong>
                        </p>

                        <p>
                            {registerResult.fileHash}
                        </p>

                    </div>
                )}

            </section>


            <hr />


            {/* -----------------------------------------
                CHECK FILE INTEGRITY
            ----------------------------------------- */}

            <section>

                <h2>Check File Integrity</h2>

                <p>
                    Upload a file and compare it against
                    a previously registered fingerprint.
                </p>


                <form onSubmit={handleCheck}>

                    <div>

                        <label htmlFor="checkFile">
                            Select File:
                        </label>

                        <input
                            type="file"
                            id="checkFile"
                            onChange={(event) =>
                                setCheckFile(
                                    event.target.files[0]
                                )
                            }
                        />

                    </div>


                    <div>

                        <label htmlFor="selectedRecord">
                            Select Registered Fingerprint:
                        </label>

                        <select
                            id="selectedRecord"
                            value={selectedRecordId}
                            onChange={(event) =>
                                setSelectedRecordId(
                                    event.target.value
                                )
                            }
                        >

                            <option value="">
                                -- Select a fingerprint --
                            </option>

                            {registeredFiles.map((file) => (

                                <option
                                    key={file.id}
                                    value={file.id}
                                >
                                    {file.fileName}
                                    {" - "}
                                    {file.algorithm}
                                    {" - ID "}
                                    {file.id}
                                </option>

                            ))}

                        </select>

                    </div>


                    <button
                        type="submit"
                        disabled={loading}
                    >
                        {loading
                            ? "Checking..."
                            : "Check Integrity"}
                    </button>

                </form>


                {/* Integrity Check Result */}

                {checkResult && (

                    <div>

                        <h3>
                            Integrity Check Result
                        </h3>


                        <p>
                            <strong>File:</strong>{" "}
                            {checkResult.fileName}
                        </p>

                        <p>
                            <strong>File Size:</strong>{" "}
                            {formatFileSize(
                                checkResult.fileSize
                            )}
                        </p>

                        <p>
                            <strong>Algorithm:</strong>{" "}
                            {checkResult.algorithm}
                        </p>


                        <p>
                            <strong>Expected Hash:</strong>
                        </p>

                        <p>
                            {checkResult.expectedHash}
                        </p>


                        <p>
                            <strong>Generated Hash:</strong>
                        </p>

                        <p>
                            {checkResult.generatedHash}
                        </p>


                        <p>
                            <strong>Result:</strong>{" "}
                            {checkResult.isMatch
                                ? "MATCH - File integrity verified."
                                : "MISMATCH - File may have been modified."}
                        </p>


                        <p>
                            {checkResult.message}
                        </p>

                    </div>
                )}

            </section>


            <hr />


            {/* -----------------------------------------
                REGISTERED FILE FINGERPRINTS
                THIS IS NOW THE LAST SECTION
            ----------------------------------------- */}

            <section>

                <h2>Registered File Fingerprints</h2>

                <p>
                    These are the file fingerprints currently
                    stored in the IceKit database.
                </p>


                {registeredFiles.length === 0 ? (

                    <p>
                        No file fingerprints have been registered yet.
                    </p>

                ) : (

                    <div>

                        <table>

                            <thead>

                                <tr>

                                    <th>ID</th>

                                    <th>File Name</th>

                                    <th>Algorithm</th>

                                    <th>File Size</th>

                                    <th>Fingerprint</th>

                                    <th>Registered</th>

                                </tr>

                            </thead>


                            <tbody>

                                {registeredFiles.map((file) => (

                                    <tr key={file.id}>

                                        <td>
                                            {file.id}
                                        </td>

                                        <td>
                                            {file.fileName}
                                        </td>

                                        <td>
                                            {file.algorithm}
                                        </td>

                                        <td>
                                            {formatFileSize(
                                                file.fileSize
                                            )}
                                        </td>

                                        <td>
                                            {file.fileHash}
                                        </td>

                                        <td>
                                            {new Date(
                                                file.createdAt
                                            ).toLocaleString()}
                                        </td>

                                    </tr>

                                ))}

                            </tbody>

                        </table>

                    </div>

                )}

            </section>

        </div>
    );
}

export default FileIntegrity;