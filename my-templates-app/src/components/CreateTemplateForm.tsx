import { useState } from 'react';
import type {CreateTemplateDto} from '../types';
import { TemplaterApi } from '../api/templaterApi';
import './CreateTemplateForm.css';

interface CreateTemplateFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

export function CreateTemplateForm({ onSuccess, onCancel }: CreateTemplateFormProps) {
  const [formData, setFormData] = useState<CreateTemplateDto>({
    name: '',
    htmlContent: ''
  });
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.name.trim() || !formData.htmlContent.trim()) {
      alert('Fill in all fields');
      return;
    }

    try {
      setLoading(true);
      await TemplaterApi.createTemplate(formData);
      onSuccess();
    } catch (err) {
      console.error('Template creation error:', err);
      alert(`Template creation error: ${err instanceof Error ? err.message : 'Unknown error'}`);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <div className="create-template-form">
      <h2>Create a new template</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Template name:</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="Enter template name"
            disabled={loading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="htmlContent">HTML content:</label>
          <textarea
            id="htmlContent"
            name="htmlContent"
            value={formData.htmlContent}
            onChange={handleChange}
            placeholder={`Example:
<h1>Welcome!</h1>
<p>Hello, @Model["name"]!</p>
<p>Your age: @Model["age"]</p>
<div>Company: @Model["company"]</div>`}
            rows={10}
            disabled={loading}
          />
          <div className="form-help">
            <p><strong>Requirements:</strong></p>
            <ul>
              <li>Must contain HTML tags (eg: &lt;div&gt;, &lt;p&gt;, &lt;h1&gt;)</li>
              <li>Must contain placeholders in the format @Model["key"]</li>
            </ul>
          </div>
        </div>

        <div className="form-actions">
          <button type="button" onClick={onCancel} disabled={loading} className="btn btn-cancel">
            Cancel
          </button>
          <button type="submit" disabled={loading} className="btn btn-submit">
            {loading ? 'Creating...' : 'Create'}
          </button>
        </div>
      </form>
    </div>
  );
}
