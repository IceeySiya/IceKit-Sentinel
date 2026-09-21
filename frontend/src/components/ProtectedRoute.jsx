import { Navigate, Outlet } from "react-router-dom";

function ProtectedRoute() {
    const token = localStorage.getItem("token");

    // If there is no JWT, the user is not logged in.
    if (!token) {
        return <Navigate to="/login" replace />;
    }

    // Render whichever protected route was requested.
    return <Outlet />;
}

export default ProtectedRoute;