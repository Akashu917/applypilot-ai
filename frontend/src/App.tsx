import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { ProtectedRoute } from './components/common/ProtectedRoute';
import { DashboardLayout } from './components/layout/DashboardLayout';
import { LandingPage } from './pages/LandingPage';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';
import { DashboardPage } from './pages/DashboardPage';
import { ProfilePage } from './pages/ProfilePage';

export const App: React.FC = () => {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          {/* Public Marketing & Auth Routes */}
          <Route path="/" element={<LandingPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          {/* Authenticated Candidate Dashboard Routes */}
          <Route element={<ProtectedRoute />}>
            <Route element={<DashboardLayout />}>
              <Route path="/dashboard" element={<DashboardPage />} />
              <Route path="/profile" element={<ProfilePage />} />
              {/* Placeholders for upcoming phases */}
              <Route
                path="/resume"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">Master Resume Management</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 2: PDF/DOCX Parser & Entity Profile Editor</p>
                  </div>
                }
              />
              <Route
                path="/jobs"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">Job Discovery Engine</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 4: Authorized Feeds & Search Engine</p>
                  </div>
                }
              />
              <Route
                path="/analyzer"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">AI Job Description Analyzer</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 3: Semantic Match & ATS Keyword Analysis</p>
                  </div>
                }
              />
              <Route
                path="/applications"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">Centralized Application Tracker</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 5: 14-State Kanban & Assisted Apply Tray</p>
                  </div>
                }
              />
              <Route
                path="/interview"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">AI Interview Preparation</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 7: Mock Q&A Evaluator</p>
                  </div>
                }
              />
              <Route
                path="/billing"
                element={
                  <div className="p-8 bg-white rounded-3xl border border-slate-200 text-center">
                    <h2 className="text-xl font-bold text-slate-800">Subscription & Billing</h2>
                    <p className="text-sm text-slate-500 mt-2">Incoming in Phase 8: Razorpay (INR) & Stripe (USD)</p>
                  </div>
                }
              />
            </Route>
          </Route>

          {/* Catch-all redirect */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
};

export default App;

