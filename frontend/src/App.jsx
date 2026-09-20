import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard.jsx";

import PasswordAnalyzer from "./pages/PasswordAnalyzer.jsx";
import PasswordGenerator from "./pages/PasswordGenerator.jsx";
import Hashing from "./pages/Hashing.jsx";
import HashVerification from "./pages/HashVerification.jsx";
import FileIntegrity from "./pages/FileIntegrity.jsx";
import SecurityHeaders from "./pages/SecurityHeaders.jsx";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route 
                    path="/"
                    element={<Navigate to="/login" replace />}
                />
                
                <Route
                    path="/register"
                    element={<Register />}
                />

                 <Route
                    path="/login"
                    element={<Login />}
                />

                <Route
                    path="/dashboard"
                    element={<Dashboard />}
                />

                 <Route
                    path="/tools/password-analyzer"
                    element={<PasswordAnalyzer />}
                />

                <Route
                    path="/tools/password-generator"
                    element={<PasswordGenerator />}
                />

                <Route
                    path="/tools/hashing"
                    element={<Hashing />}
                />

                <Route
                    path="/tools/hash-verification"
                    element={<HashVerification />}
                />

                <Route
                    path="/tools/file-integrity"
                    element={<FileIntegrity />}
                />

                <Route
                    path="/tools/security-headers"
                    element={<SecurityHeaders />}
                />
            </Routes>
        </BrowserRouter>
    );
}

export default App;