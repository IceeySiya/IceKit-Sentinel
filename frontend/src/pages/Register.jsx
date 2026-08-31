import { useState } from "react";
import { registerUser } from "../services/authService";

function Register(){
    const [formData, setFormData] = useState({
        username: "",
        email: "",
        password: ""
    });

    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    function handleChange(event){
          setFormData({
            ...formData,
            [event.target.name]: event.target.value
        });
    }

    async function handleSubmit(event){
        event.preventDefault();

        setMessage("");
        setError("");
        try{
            const results= await registerUser(formData);
            setMessage(results.message);
            setFormData({
                username: "",
                email: "",
                password: ""
            });
        } catch (error){
            setError(error.message);
        }

    }

    return(
        <div>
            <h1>Create Account</h1>

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        value={formData.username}
                        onChange={handleChange}
                        name="username"
                        required
                    />
                </div>

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

                <button type="submit">Create Account</button>
            </form>

             {message && <p>{message}</p>}

            {error && <p>{error}</p>}
        </div>
        
    );

}
export default Register;