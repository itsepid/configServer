import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './ConfigPage.css';

const ConfigPage = () => {
  const [configs, setConfigs] = useState([]);
  const [editingConfig, setEditingConfig] = useState(null); // For handling updates
  const [newConfig, setNewConfig] = useState({
    key: '',
    value: '',
    description: '',
    projectId: '',
    file: null,
  });

  useEffect(() => {
    const fetchConfigs = async () => {
      try {
        const response = await axios.get('http://localhost:5127/api/config', {
          withCredentials: true,
        });
        setConfigs(response.data);
      } catch (error) {
        console.error('Error fetching configurations:', error);
      }
    };
    fetchConfigs();
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewConfig((prev) => ({ ...prev, [name]: value }));
  };

  const handleFileChange = (e) => {
    setNewConfig((prev) => ({ ...prev, file: e.target.files[0] }));
  };

  const handleCreateConfig = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append('Key', newConfig.key);
    formData.append('Value', newConfig.value);
    formData.append('Description', newConfig.description);
    formData.append('ProjectId', newConfig.projectId);
    formData.append('File', newConfig.file);

    try {
      const response = await axios.post('http://localhost:5127/api/config', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
        withCredentials: true,
      });
      setConfigs([...configs, response.data]);
      setNewConfig({ key: '', value: '', description: '', projectId: '', file: null });
      alert('Configuration created successfully!');
    } catch (error) {
      alert('Error creating configuration.');
    }
  };

  const handleDelete = (id) => {
    setConfigs(configs.filter((config) => config.id !== id));
    alert('Configuration deleted from the list.');
  };

  const handleUpdate = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append('Key', newConfig.key);
    formData.append('Value', newConfig.value);
    formData.append('Description', newConfig.description);
    formData.append('ProjectId', newConfig.projectId);
    if (newConfig.file) formData.append('File', newConfig.file);

    try {
      await axios.put(`http://localhost:5127/api/config/${editingConfig.id}`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
        withCredentials: true,
      });

      alert('Configuration updated successfully!');
      setConfigs(
        configs.map((config) =>
          config.id === editingConfig.id ? { ...editingConfig, ...newConfig } : config
        )
      );
      setEditingConfig(null);
      setNewConfig({ key: '', value: '', description: '', projectId: '', file: null });
    } catch (error) {
      alert('Error updating configuration.');
    }
  };

  const startEditing = (config) => {
    setEditingConfig(config);
    setNewConfig({
      key: config.key,
      value: config.value,
      description: config.description,
      projectId: config.projectId,
      file: null,
    });
  };

  return (
    <div className="config-page">
      <h2>Configurations</h2>
      <ul className="config-list">
        {configs.map((config) => (
          <li key={config.id} className="config-item">
            <p><strong>ID:</strong> {config.id}</p>
            <p><strong>Key:</strong> {config.key}</p>
            <p><strong>Value:</strong> {config.value}</p>
            <p><strong>Description:</strong> {config.description}</p>
            <p><strong>Project ID:</strong> {config.projectId}</p>
            <p><strong>Created At:</strong> {new Date(config.createdAt).toLocaleString()}</p>
            <p><strong>Updated At:</strong> {new Date(config.updatedAt).toLocaleString()}</p>
            <p><strong>File Path:</strong> {config.filePath}</p>
            <p><strong>File URL:</strong> <a href={config.fileUrl} target="_blank" rel="noopener noreferrer">{config.fileUrl}</a></p>

            <div className="action-buttons">
              <button className="update-btn" onClick={() => startEditing(config)}>Update</button>
              <button className="delete-btn" onClick={() => handleDelete(config.id)}>Delete</button>
            </div>
          </li>
        ))}
      </ul>

      <form onSubmit={editingConfig ? handleUpdate : handleCreateConfig}>
        <h3>{editingConfig ? 'Update Configuration' : 'Create New Configuration'}</h3>
        <input
          type="text"
          name="key"
          placeholder="Key"
          value={newConfig.key}
          onChange={handleInputChange}
          required
        />
        <input
          type="text"
          name="value"
          placeholder="Value"
          value={newConfig.value}
          onChange={handleInputChange}
          required
        />
        <textarea
          name="description"
          placeholder="Description"
          value={newConfig.description}
          onChange={handleInputChange}
        />
        <input
          type="text"
          name="projectId"
          placeholder="Project ID"
          value={newConfig.projectId}
          onChange={handleInputChange}
          required
        />
        <input type="file" name="file" onChange={handleFileChange} />

        <button type="submit">{editingConfig ? 'Update Config' : 'Create Config'}</button>
      </form>
    </div>
  );
};

export default ConfigPage;
