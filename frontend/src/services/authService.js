const API_URL = "http://Localhost:5131/api/auth";

/*
 Register a user via Asp.NET Core API
*/
export async function registerUser(userData){
    const response = await fetch(`${API_URL}/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userData)
    });

     const data = await response.json();

     if(!response.ok){
        throw new Error(data.message || "Failed to register user");
     }
     return data;
}

/*
    Login a user via Asp.NET Core API
*/

export async function loginUser(userData){
    const response= await fetch(`${API_URL}/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userData)
    });

    const data = await response.json();

    if(!response.ok){
        throw new Error(data.message || "Failed to login user");
    }
    return data;
}
