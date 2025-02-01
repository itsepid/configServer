import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Home from './components/Home';
import Register from './components/Register';
import Login from './components/Login';
import ConfigList from './components/ConfigPage';
import CreateConfig from './components/CreateConfig';
import './App.css';

const API_BASE_URL = "http://localhost:5127/api";  // Change to your backend URL

const App = () => {
  return (
    <Router>
        <div className="content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route path="/configs" element={<ConfigList />} />
            <Route path="/create-config" element={<CreateConfig />} />
          </Routes>
        </div>
    </Router>
  );
};

export default App;
