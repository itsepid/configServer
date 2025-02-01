import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Home.css';

const Home = () => {
  const [isLogin, setIsLogin] = useState(true);
  const navigate = useNavigate();

  const [loginData, setLoginData] = useState({ username: '', password: '' });
  const [registerData, setRegisterData] = useState({ username: '', password: '', role: '' });

  const handleToggle = () => setIsLogin(!isLogin);

  const handleInputChange = (e, setData) => {
    setData((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5127/api/user/login', loginData, { withCredentials: true });
      alert('Login successful!');
      navigate('/configs');
    } catch (error) {
      alert('Error during login.');
    }
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5127/api/user/register', registerData);
      alert('Registration successful!');
      navigate('/configs');
    } catch (error) {
      alert('Error during registration.');
    }
  };

  return (
    <div className="home-container">
      <div className="form-box">
        <h2>{isLogin ? 'Login' : 'Register'}</h2>

        {isLogin ? (
          <form onSubmit={handleLoginSubmit}>
            <input
              type="text"
              name="username"
              placeholder="Username"
              value={loginData.username}
              onChange={(e) => handleInputChange(e, setLoginData)}
              required
            />
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={loginData.password}
              onChange={(e) => handleInputChange(e, setLoginData)}
              required
            />
            <button type="submit">Login</button>
          </form>
        ) : (
          <form onSubmit={handleRegisterSubmit}>
            <input
              type="text"
              name="username"
              placeholder="Username"
              value={registerData.username}
              onChange={(e) => handleInputChange(e, setRegisterData)}
              required
            />
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={registerData.password}
              onChange={(e) => handleInputChange(e, setRegisterData)}
              required
            />
            <input
              type="text"
              name="role"
              placeholder="Role (e.g., Admin)"
              value={registerData.role}
              onChange={(e) => handleInputChange(e, setRegisterData)}
              required
            />
            <button type="submit">Register</button>
          </form>
        )}

        <p onClick={handleToggle} className="toggle-link">
          {isLogin ? "Don't have an account? Register" : 'Already have an account? Login'}
        </p>
      </div>
    </div>
  );
};

export default Home;
