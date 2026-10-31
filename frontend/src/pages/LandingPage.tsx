import React from 'react';
import { Link } from 'react-router-dom';
import { Navbar } from '../components/layout/Navbar';
import {
  Sparkles,
  ArrowRight,
  ShieldCheck,
  CheckCircle2,
  FileCheck,
  Target,
  Layers,
  Globe2
} from 'lucide-react';

export const LandingPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-slate-50 flex flex-col">
      <Navbar />

      {/* Hero Section */}
      <section className="relative overflow-hidden pt-16 pb-20 sm:pt-24 sm:pb-32 bg-gradient-to-b from-white to-slate-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center relative z-10">
          <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-brand-50 border border-brand-200/60 text-brand-700 text-xs font-semibold mb-6 shadow-sm">
            <Sparkles className="w-4 h-4 text-brand-600" />
            <span>AI Job Search & Application Automation SaaS</span>
          </div>

          <h1 className="text-4xl sm:text-6xl lg:text-7xl font-extrabold tracking-tight text-slate-900 max-w-4xl mx-auto leading-[1.1]">
            One Resume. Multiple Jobs.{' '}
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-brand-600 via-indigo-600 to-brand-500">
              Smarter Applications.
            </span>
          </h1>

          <p className="mt-6 text-lg sm:text-xl text-slate-600 max-w-2xl mx-auto leading-relaxed">
            Stop rewriting resumes for every portal. Upload your verified Master Resume once. ApplyPilot AI analyzes job descriptions, produces tailored ATS resumes, and guides error-free submissions across Indian & Global job markets.
          </p>

          <div className="mt-10 flex flex-col sm:flex-row items-center justify-center gap-4">
            <Link
              to="/register"
              className="w-full sm:w-auto inline-flex items-center justify-center gap-2 px-8 py-4 rounded-2xl bg-brand-600 hover:bg-brand-700 text-white font-bold text-base shadow-lg shadow-brand-500/25 hover:shadow-xl hover:shadow-brand-500/30 transition-all duration-200"
            >
              <span>Get Started Free</span>
              <ArrowRight className="w-5 h-5" />
            </Link>
            <Link
              to="/login"
              className="w-full sm:w-auto inline-flex items-center justify-center gap-2 px-8 py-4 rounded-2xl bg-white hover:bg-slate-50 text-slate-700 font-bold text-base border border-slate-200 shadow-sm transition-all duration-200"
            >
              <span>Sign In to Dashboard</span>
            </Link>
          </div>

          {/* Social Proof & Guarantees */}
          <div className="mt-12 flex flex-wrap items-center justify-center gap-6 text-xs font-medium text-slate-500">
            <div className="flex items-center gap-1.5">
              <ShieldCheck className="w-4 h-4 text-emerald-600" />
              <span>Zero Account Bans (Assisted Handoff)</span>
            </div>
            <div className="flex items-center gap-1.5">
              <CheckCircle2 className="w-4 h-4 text-brand-600" />
              <span>Zero Fake Experience or Hallucination</span>
            </div>
            <div className="flex items-center gap-1.5">
              <Globe2 className="w-4 h-4 text-indigo-600" />
              <span>Built for India (Naukri, Foundit) & Global</span>
            </div>
          </div>
        </div>
      </section>

      {/* Feature Cards Grid */}
      <section className="py-20 bg-white border-y border-slate-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center max-w-3xl mx-auto mb-16">
            <h2 className="text-xs font-bold uppercase tracking-widest text-brand-600 mb-3">
              Autonomous Candidate Platform
            </h2>
            <p className="text-3xl font-extrabold text-slate-900 tracking-tight">
              Engineered to Solve the Real Friction in Modern Job Hunting
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="p-8 rounded-3xl bg-slate-50 border border-slate-200/80 hover:border-brand-300 transition-all">
              <div className="w-12 h-12 rounded-2xl bg-brand-100 text-brand-700 flex items-center justify-center mb-6">
                <FileCheck className="w-6 h-6" />
              </div>
              <h3 className="text-xl font-bold text-slate-900 mb-2">
                Verified Master Resume
              </h3>
              <p className="text-sm text-slate-600 leading-relaxed">
                Upload your root PDF/DOCX once. Our entity parser maps your verified skills, experience, and achievements as the single factual source of truth.
              </p>
            </div>

            <div className="p-8 rounded-3xl bg-slate-50 border border-slate-200/80 hover:border-brand-300 transition-all">
              <div className="w-12 h-12 rounded-2xl bg-indigo-100 text-indigo-700 flex items-center justify-center mb-6">
                <Target className="w-6 h-6" />
              </div>
              <h3 className="text-xl font-bold text-slate-900 mb-2">
                ATS Compatibility & Semantic Gap
              </h3>
              <p className="text-sm text-slate-600 leading-relaxed">
                Compare job descriptions against your verified profile. Get transparent match percentages, missing skill checklists, and keyword alignment.
              </p>
            </div>

            <div className="p-8 rounded-3xl bg-slate-50 border border-slate-200/80 hover:border-brand-300 transition-all">
              <div className="w-12 h-12 rounded-2xl bg-emerald-100 text-emerald-700 flex items-center justify-center mb-6">
                <Layers className="w-6 h-6" />
              </div>
              <h3 className="text-xl font-bold text-slate-900 mb-2">
                One-Click Tailored Resumes
              </h3>
              <p className="text-sm text-slate-600 leading-relaxed">
                Generate derived resume versions framed specifically for each role. Download clean, ATS-compliant PDFs with side-by-side diff verification.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Pricing Hypothesis */}
      <section className="py-20 bg-slate-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-xs font-bold uppercase tracking-widest text-brand-600 mb-3">
            Transparent SaaS Plans
          </h2>
          <p className="text-3xl font-extrabold text-slate-900 tracking-tight mb-12">
            Affordable for Fresher Candidates & Senior Professionals
          </p>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto text-left">
            {/* Free */}
            <div className="p-8 rounded-3xl bg-white border border-slate-200 shadow-sm flex flex-col justify-between">
              <div>
                <span className="text-xs font-bold uppercase tracking-wider text-slate-500">Starter</span>
                <div className="mt-4 flex items-baseline gap-1">
                  <span className="text-4xl font-extrabold text-slate-900">₹0</span>
                  <span className="text-sm text-slate-500">/ month</span>
                </div>
                <p className="mt-2 text-xs text-slate-500">Perfect to test the resume parser and analyzer.</p>
                <ul className="mt-6 space-y-3 text-xs text-slate-600">
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>1 Master Resume Profile</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>5 Job Description Match Analyses / mo</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>Basic Application Tracker</span>
                  </li>
                </ul>
              </div>
              <Link
                to="/register"
                className="mt-8 block w-full py-3 rounded-xl bg-slate-100 hover:bg-slate-200 text-slate-800 text-center text-xs font-bold transition-all"
              >
                Sign Up Free
              </Link>
            </div>

            {/* Pro */}
            <div className="p-8 rounded-3xl bg-brand-900 text-white border-2 border-brand-500 shadow-xl relative flex flex-col justify-between">
              <span className="absolute -top-3 right-6 bg-brand-500 text-white text-[10px] font-extrabold uppercase px-3 py-1 rounded-full shadow-md">
                Most Popular
              </span>
              <div>
                <span className="text-xs font-bold uppercase tracking-wider text-brand-200">Pro Seeker</span>
                <div className="mt-4 flex items-baseline gap-1">
                  <span className="text-4xl font-extrabold text-white">₹199</span>
                  <span className="text-sm text-brand-200">/ month (\$5 USD)</span>
                </div>
                <p className="mt-2 text-xs text-brand-200">For active applicants submitting weekly applications.</p>
                <ul className="mt-6 space-y-3 text-xs text-brand-100">
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-brand-400 flex-shrink-0" />
                    <span>Unlimited Master Resume updates</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-brand-400 flex-shrink-0" />
                    <span>50 AI Resume Optimizations / mo</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-brand-400 flex-shrink-0" />
                    <span>Cover Letter Generator</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-brand-400 flex-shrink-0" />
                    <span>Assisted Handoff Workflow Tray</span>
                  </li>
                </ul>
              </div>
              <Link
                to="/register"
                className="mt-8 block w-full py-3 rounded-xl bg-brand-500 hover:bg-brand-400 text-white text-center text-xs font-bold shadow-md transition-all"
              >
                Start Pro Access
              </Link>
            </div>

            {/* Premium */}
            <div className="p-8 rounded-3xl bg-white border border-slate-200 shadow-sm flex flex-col justify-between">
              <div>
                <span className="text-xs font-bold uppercase tracking-wider text-slate-500">Premium Autopilot</span>
                <div className="mt-4 flex items-baseline gap-1">
                  <span className="text-4xl font-extrabold text-slate-900">₹499</span>
                  <span className="text-sm text-slate-500">/ month (\$12 USD)</span>
                </div>
                <p className="mt-2 text-xs text-slate-500">High-volume search with AI mock interviews.</p>
                <ul className="mt-6 space-y-3 text-xs text-slate-600">
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>All Pro features included</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>200 AI Optimizations / mo</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>Interactive AI Mock Interviews</span>
                  </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle2 className="w-4 h-4 text-emerald-600 flex-shrink-0" />
                    <span>Priority Queue Processing</span>
                  </li>
                </ul>
              </div>
              <Link
                to="/register"
                className="mt-8 block w-full py-3 rounded-xl bg-slate-900 hover:bg-slate-800 text-white text-center text-xs font-bold transition-all"
              >
                Join Premium
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="mt-auto py-8 bg-white border-t border-slate-200 text-center text-xs text-slate-500">
        <div className="max-w-7xl mx-auto px-4 flex flex-col sm:flex-row items-center justify-between gap-4">
          <p>© 2026 ApplyPilot AI. All rights reserved. Compliant with India DPDP Act 2023 & GDPR.</p>
          <div className="flex gap-6">
            <span className="hover:text-slate-800 cursor-pointer">Privacy Policy</span>
            <span className="hover:text-slate-800 cursor-pointer">Terms of Service</span>
            <span className="hover:text-slate-800 cursor-pointer">Security Safeguards</span>
          </div>
        </div>
      </footer>
    </div>
  );
};
