import { useState, useEffect } from 'react';
import type {Template, UpdateTemplateDto} from '../types';
import { TemplaterApi } from '../api/templaterApi';
import './EditTemplateForm.css';

interface EditTemplateFormProps {
  template: Template;
  onSuccess: () => void;
  onCancel: () => void;
}

export function EditTemplateForm({ template, onSuccess, onCancel }: EditTemplateFormProps) {
  const [formData, setFormData] = useState<UpdateTemplateDto>({
    name: template.name,
    htmlContent: template.htmlContent
  });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setFormData({
      name: template.name,
      htmlContent: template.htmlContent
    });
  }, [template]);

  const validateForm = (): string | null => {
    if (!formData.name.trim()) {
      return 'Name is required';
    }
    
    if (formData.name.length > 50) {
      return 'Name must not exceed 50 characters';
    }
    
    if (!formData.htmlContent.trim()) {
      return 'HTML content cannot be empty';
    }
    
    // Check for HTML tags
    const htmlTagRegex = /<.*?>/;
    if (!htmlTagRegex.test(formData.htmlContent)) {
      return 'HTML content must contain at least one HTML tag (e.g., <div>, <p>, <h1>)';
    }
    
    // Check for Razor placeholders
    const razorPlaceholderRegex = /@Model\[".+?"\]/;
    if (!razorPlaceholderRegex.test(formData.htmlContent)) {
      return 'HTML content must contain at least one Razor placeholder like @Model["key"]';
    }
    
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const validationError = validateForm();
    if (validationError) {
      alert(validationError);
      return;
    }

    try {
      setLoading(true);
      await TemplaterApi.updateTemplate(template.id, formData);
      onSuccess();
    } catch (err) {
      console.error('Template update error:', err);
      const errorMessage = err instanceof Error ? err.message : 'Unknown error';
      alert(`Template update error: ${errorMessage}`);
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
    <div className="edit-template-form">
      <h2>Edit template</h2>
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
<div>Company: @Model["company"]</div>
<p>Thank you for using our service!</p>`}
            rows={10}
            disabled={loading}
          />
          <div className="form-help">
            <p><strong>Требования:</strong></p>
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
            {loading ? 'Saving...' : 'Save'}
          </button>
        </div>
      </form>
    </div>
  );
}
