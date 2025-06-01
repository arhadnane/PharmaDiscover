# AI-Based Drug Discovery Simulator

An advanced web application built with ASP.NET Core 8.0 and Blazor Server that simulates AI-driven drug discovery processes. The application features random molecule generation, AI-based therapeutic scoring, comprehensive data management, and detailed analytics.

## 🚀 Features

### Core Functionality
- **Random Molecule Generation**: Generate realistic chemical compounds with customizable parameters
- **AI Therapeutic Scoring**: Advanced scoring algorithm based on Lipinski's Rule of Five and drug-likeness criteria
- **Molecule Database**: Complete CRUD operations with SQLite storage
- **Search & Filtering**: Advanced search capabilities with real-time filtering
- **Data Analytics**: Comprehensive statistics and data visualization dashboard
- **CSV Export**: Export molecule data for external analysis

### Technical Features
- **RESTful API**: Complete API with Swagger documentation
- **Real-time Updates**: Blazor Server for dynamic user interface
- **Responsive Design**: Bootstrap-based UI optimized for all devices
- **Performance Optimized**: Database indexing and pagination support
- **Comprehensive Logging**: Structured logging with Serilog

## 🧬 Molecule Generation

The application generates realistic molecules with the following properties:

### Chemical Properties
- **Molecular Weight**: 150-600 Da range
- **LogP (Lipophilicity)**: -2 to 6 range for drug-like properties
- **Hydrogen Bond Donors/Acceptors**: Realistic counts for drug compounds
- **Atomic Composition**: Carbon, hydrogen, nitrogen, oxygen atoms
- **Rotatable Bonds**: Flexibility indicators
- **Polar Surface Area**: Membrane permeability predictions

### SMILES Generation
- Generates chemically valid SMILES notation
- Includes various functional groups (alcohols, amines, carboxylic acids, etc.)
- Supports aromatic and aliphatic structures

## 🤖 AI Scoring Algorithm

The therapeutic scoring system evaluates molecules based on multiple criteria:

### Drug Likeness Score (Lipinski's Rule of Five)
- **Molecular Weight**: ≤ 500 Da
- **LogP**: ≤ 5
- **Hydrogen Bond Donors**: ≤ 5
- **Hydrogen Bond Acceptors**: ≤ 10

### Additional Scoring Factors
- **Toxicity Prediction**: Based on structural alerts and properties
- **Solubility Assessment**: Aqueous solubility predictions
- **Synthetic Accessibility**: Ease of chemical synthesis
- **Overall Therapeutic Potential**: Weighted combination of all factors

## 📊 Analytics Dashboard

### Statistics Overview
- Total molecules generated
- Average therapeutic scores
- Distribution of molecular properties
- Top-performing compounds

### Visualizations
- Score distribution histograms
- Molecular weight distributions
- LogP vs. score correlations
- Property correlation matrices

## 🛠️ Technology Stack

### Backend
- **ASP.NET Core 8.0**: Modern web framework
- **Entity Framework Core**: ORM for database operations
- **SQLite**: Lightweight, file-based database
- **Serilog**: Structured logging framework

### Frontend
- **Blazor Server**: Server-side rendering with real-time updates
- **Bootstrap 5**: Responsive CSS framework
- **Chart.js**: Interactive data visualizations
- **Custom CSS**: Pharmaceutical-themed styling

### API Documentation
- **Swagger/OpenAPI**: Comprehensive API documentation
- **RESTful Endpoints**: Standard HTTP operations
- **JSON Responses**: Structured data exchange

## 📋 API Endpoints

### Molecule Management
```
GET    /api/molecules              - Get paginated molecules
POST   /api/molecules              - Create new molecule
GET    /api/molecules/{id}         - Get specific molecule
PUT    /api/molecules/{id}         - Update molecule
DELETE /api/molecules/{id}         - Delete molecule
```

### Generation & Analytics
```
POST   /api/molecules/generate     - Generate random molecules
GET    /api/molecules/search       - Search molecules
GET    /api/molecules/statistics   - Get analytics data
GET    /api/molecules/export       - Export molecules as CSV
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Modern web browser

### Installation
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd MoleculeSimulator
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the application:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to:
   ```
   http://localhost:5142
   ```

### Database
The application uses SQLite with automatic database creation. The database file (`molecules.db`) will be created in the project root on first run.

## 📱 User Interface

### Home Page
- Welcome message and feature overview
- Navigation to key sections
- Quick access to generation and analysis tools

### Molecule Generator
- Customizable generation parameters
- Batch generation capabilities
- Real-time preview of generated molecules
- Export functionality

### Molecule Browser
- Searchable table of all molecules
- Detailed molecule information modals
- Sorting and filtering options
- Score-based categorization

### Statistics Dashboard
- Comprehensive analytics
- Interactive charts and graphs
- Export capabilities
- Data insights and trends

## 🔬 Scientific Accuracy

The application implements established pharmaceutical principles:

### Lipinski's Rule of Five
Industry-standard criteria for drug-like properties, ensuring generated molecules have realistic pharmaceutical characteristics.

### ADMET Properties
Consideration of Absorption, Distribution, Metabolism, Excretion, and Toxicity factors in the scoring algorithm.

### Chemical Validity
SMILES generation follows chemical rules and produces valid molecular structures.

## 🛡️ Performance & Scalability

### Database Optimization
- Indexed columns for fast querying
- Pagination for large datasets
- Efficient search algorithms

### Caching Strategy
- In-memory caching for frequently accessed data
- Optimized database queries
- Minimal network overhead

### Responsive Design
- Mobile-friendly interface
- Progressive loading
- Optimized asset delivery

## 🔧 Configuration

### Connection Strings
Configure database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=molecules.db"
  }
}
```

### Logging Configuration
Serilog is configured for comprehensive logging with different levels for development and production environments.

## 📈 Future Enhancements

### Planned Features
- Machine learning integration with ML.NET
- 3D molecular visualization
- Advanced chemical property calculations
- Integration with chemical databases
- Multi-user support with authentication
- Advanced analytics and reporting

### Extensibility
The modular architecture allows for easy extension with additional scoring algorithms, data sources, and visualization components.

## 📄 License

This project is created for educational and research purposes. Please ensure compliance with relevant regulations when working with pharmaceutical data.

## 🤝 Contributing

Contributions are welcome! Please follow the established coding standards and include appropriate tests for new features.

## 📞 Support

For questions, issues, or feature requests, please refer to the project documentation or create an issue in the repository.

---

**AI-Based Drug Discovery Simulator** - Advancing pharmaceutical research through intelligent molecular design and analysis.
