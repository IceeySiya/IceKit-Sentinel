import { useState } from "react";
import { loginUser } from "../services/authService";

function Login(){
    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });

    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    function handleChange(event) {
        setFormData({
            ...formData,
            [event.target.name]: event.target.value
        });
    }

    async function handleSubmit(event) {
        event.preventDefault();

        setMessage("");
        setError("");

        try {
            const result = await loginUser(formData);

            /*
             * The backend returns a JWT after
             * successful authentication.
             */
            localStorage.setItem("token", result.token);

            setMessage("Login successful.");
        } catch (error) {
            setError(error.message);
        }
    }

    return(
        <div>
            <h1>Login</h1>

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <input
                        type="email"
                        id="email"
                        value={formData.email}
                        onChange={handleChange}
                        name="email"
                        required
                    />
                </div>

                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        value={formData.password}
                        onChange={handleChange}
                        name="password"
                        required
                    />
                </div>

                <button type="submit">Login</button>
            </form>

            {message && <p>{message}</p>}

            {error && <p>{error}</p>}
        </div>
    );
}
export default Login;