import { useState } from 'react';
import type {Template} from '../types';
import { TemplaterApi } from '../api/templaterApi';
import './GeneratePdfForm.css';

interface GeneratePdfFormProps {
  template: Template;
  onSuccess: () => void;
  onCancel: () => void;
}

export function GeneratePdfForm({ template, onSuccess, onCancel }: GeneratePdfFormProps) {
  const [jsonData, setJsonData] = useState('{\n  "example": "value"\n}');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!jsonData.trim()) {
      alert('Enter JSON data');
      return;
    }

    let parsedData: Record<string, any>;
    try {
      parsedData = JSON.parse(jsonData);
    } catch (err) {
      setError('Invalid JSON format');
      return;
    }

    try {
      setLoading(true);
      setError(null);
      
      const pdfBlob = await TemplaterApi.generatePdf(template.id, { data: parsedData });
      
      const url = window.URL.createObjectURL(pdfBlob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `${template.name}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      
      onSuccess();
    } catch (err) {
      setError('PDF generation error');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleJsonChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setJsonData(e.target.value);
    setError(null);
  };

  return (
    <div className="generate-pdf-form">
      <h2>Generate PDF for template: {template.name}</h2>
      
      <div className="template-preview">
        <h3>Preview HTML:</h3>
        <div className="html-preview">
          <pre>{template.htmlContent}</pre>
        </div>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="jsonData">
            JSON data to be substituted into the template:
          </label>
          <textarea
            id="jsonData"
            value={jsonData}
            onChange={handleJsonChange}
            placeholder='Enter JSON data, for example: {"name": "Ivan", "age": 25}'
            rows={8}
            disabled={loading}
          />
          {error && <div className="error-message">{error}</div>}
        </div>

        <div className="form-help">
          <p>
            <strong>Advice:</strong> Use placeholders in the HTML view template <code>@Model["key"]</code>
            to substitute values from JSON.
          </p>
        </div>

        <div className="form-actions">
          <button type="button" onClick={onCancel} disabled={loading} className="btn btn-cancel">
            Cancel
          </button>
          <button type="submit" disabled={loading} className="btn btn-generate">
            {loading ? 'Generation...' : 'Generate and download PDF'}
          </button>
        </div>
      </form>
    </div>
  );
}
