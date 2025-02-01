import React, { useState, useEffect } from 'react';
import axios from 'axios';

const API_BASE_URL = "http://localhost:5127/api";  // Update as needed

const CreateConfig = () => {
    const [formData, setFormData] = useState({ key: '', value: '', file: null });
    const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });
    const handleFileChange = (e) => setFormData({ ...formData, file: e.target.files[0] });
  
    const handleSubmit = async (e) => {
      e.preventDefault();
      const formDataObj = new FormData();
      formDataObj.append('key', formData.key);
      formDataObj.append('value', formData.value);
      if (formData.file) {
        formDataObj.append('file', formData.file);
      }
  
      try {
        const response = await axios.post(`${API_BASE_URL}/config`, formDataObj, {
          headers: { 'Content-Type': 'multipart/form-data' },
          withCredentials: true,
        });
        alert('Configuration created successfully!');
      } catch (error) {
        alert(`Error: ${error.response?.data?.message || error.message}`);
      }
    };
  
    return (
      <div>
        <h2>Create Configuration</h2>
        <form onSubmit={handleSubmit}>
          <input type="text" name="key" placeholder="Key" onChange={handleChange} /><br />
          <input type="text" name="value" placeholder="Value" onChange={handleChange} /><br />
          <input type="file" name="file" onChange={handleFileChange} /><br />
          <button type="submit">Create Config</button>
        </form>
      </div>
    );
  };
  export default CreateConfig;