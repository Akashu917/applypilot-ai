import React from 'react';
import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import {
  Upload,
  Sparkles,
  ArrowRight,
  TrendingUp,
  CheckCircle2,
  Clock,
  Briefcase,
  Layers,
  MapPin
} from 'lucide-react';

export const DashboardPage: React.FC = () => {
  const { user, profile } = useAuth();

  return (
    <div className="space-y-8 max-w-6xl">
      {/* Welcome Banner */}
      <div className="p-8 rounded-3xl bg-gradient-to-r from-brand-900 via-indigo-900 to-slate-900 text-white shadow-lg relative overflow-hidden">
        <div className="relative z-10 max-w-2xl">
          <div className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-brand-500/20 text-brand-300 text-xs font-semibold mb-3 border border-brand-400/30">
            <Sparkles className="w-3.5 h-3.5" />
            <span>Candidate Cockpit — Phase 1 Active</span>
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight">
            Welcome back, {profile?.firstName || user?.email.split('@')[0]}!
          </h1>
          <p className="mt-2 text-sm text-slate-300 leading-relaxed">
            Your centralized automation hub is configured. Verified profile is anchored to {profile?.city || 'Bengaluru'}, {profile?.country || 'India'}.
          </p>

          <div className="mt-6 flex flex-wrap gap-3">
            <Link
              to="/resume"
              className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl bg-brand-500 hover:bg-brand-400 text-white text-xs font-bold shadow-md transition-all"
            >
              <Upload className="w-4 h-4" />
              <span>Upload Master Resume (Phase 2)</span>
            </Link>
            <Link
              to="/profile"
              className="inline-flex items-center gap-2 px-5 py-2.5 rounded-xl bg-white/10 hover:bg-white/20 text-white text-xs font-semibold backdrop-blur-md transition-all"
            >
              <span>Edit Candidate Profile</span>
            </Link>
          </div>
        </div>

        {/* Decorative Badge */}
        <div className="hidden lg:block absolute right-8 top-8 bottom-8 w-64 bg-white/5 border border-white/10 rounded-2xl p-5 backdrop-blur-sm">
          <div className="text-xs font-bold uppercase text-brand-300 mb-2">Profile Completeness</div>
          <div className="text-3xl font-extrabold text-white mb-2">80%</div>
          <div className="w-full bg-white/10 rounded-full h-2 mb-3">
            <div className="bg-brand-400 h-2 rounded-full w-4/5"></div>
          </div>
          <p className="text-[11px] text-slate-400">
            Add your Master Resume PDF in Phase 2 to achieve 100% verification score.
          </p>
        </div>
      </div>

      {/* Metrics Row */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
        <div className="p-5 rounded-2xl bg-white border border-slate-200/80 shadow-sm">
          <div className="flex items-center justify-between text-slate-500 text-xs font-semibold mb-2">
            <span>Saved Roles</span>
            <div className="w-8 h-8 rounded-lg bg-slate-100 flex items-center justify-center text-slate-700">
              <Briefcase className="w-4 h-4" />
            </div>
          </div>
          <div className="text-2xl font-extrabold text-slate-900">0</div>
          <span className="text-[11px] text-slate-400 mt-1 block">Awaiting Job Feeds (Phase 4)</span>
        </div>

        <div className="p-5 rounded-2xl bg-white border border-slate-200/80 shadow-sm">
          <div className="flex items-center justify-between text-slate-500 text-xs font-semibold mb-2">
            <span>Tailored Resumes</span>
            <div className="w-8 h-8 rounded-lg bg-brand-50 flex items-center justify-center text-brand-600">
              <Layers className="w-4 h-4" />
            </div>
          </div>
          <div className="text-2xl font-extrabold text-slate-900">0</div>
          <span className="text-[11px] text-brand-600 font-medium mt-1 block">AI Optimizer Engine (Phase 3)</span>
        </div>

        <div className="p-5 rounded-2xl bg-white border border-slate-200/80 shadow-sm">
          <div className="flex items-center justify-between text-slate-500 text-xs font-semibold mb-2">
            <span>Applications Tracked</span>
            <div className="w-8 h-8 rounded-lg bg-indigo-50 flex items-center justify-center text-indigo-600">
              <Clock className="w-4 h-4" />
            </div>
          </div>
          <div className="text-2xl font-extrabold text-slate-900">0</div>
          <span className="text-[11px] text-slate-400 mt-1 block">Assisted Handoff Ready</span>
        </div>

        <div className="p-5 rounded-2xl bg-white border border-slate-200/80 shadow-sm">
          <div className="flex items-center justify-between text-slate-500 text-xs font-semibold mb-2">
            <span>Interview Rate</span>
            <div className="w-8 h-8 rounded-lg bg-emerald-50 flex items-center justify-center text-emerald-600">
              <TrendingUp className="w-4 h-4" />
            </div>
          </div>
          <div className="text-2xl font-extrabold text-slate-900">0%</div>
          <span className="text-[11px] text-emerald-600 font-medium mt-1 block">Telemetry Active</span>
        </div>
      </div>

      {/* Main Roadmap & Activity Section */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-6">
          <div className="p-6 rounded-3xl bg-white border border-slate-200/80 shadow-sm">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-base font-bold text-slate-900">
                10-Phase Engineering Roadmap Progress
              </h2>
              <span className="text-xs font-bold text-emerald-700 bg-emerald-50 px-2.5 py-1 rounded-full border border-emerald-200/60">
                Phase 1 Verified Complete
              </span>
            </div>

            <div className="space-y-3">
              <div className="p-4 rounded-2xl bg-emerald-50/50 border border-emerald-200/60 flex items-start gap-3">
                <CheckCircle2 className="w-5 h-5 text-emerald-600 flex-shrink-0 mt-0.5" />
                <div>
                  <h3 className="text-xs font-bold text-slate-900">Phase 1: Foundation & Authentication</h3>
                  <p className="text-xs text-slate-600 mt-0.5">
                    Clean Architecture (.NET 8/10 + PostgreSQL + React 19), JWT Auth with rotating refresh tokens, profile management, and unit/integration test suites passing 100%.
                  </p>
                </div>
              </div>

              <div className="p-4 rounded-2xl bg-brand-50/40 border border-brand-200/60 flex items-start gap-3">
                <div className="w-5 h-5 rounded-full border-2 border-brand-500 flex items-center justify-center text-brand-600 text-[10px] font-bold mt-0.5">
                  2
                </div>
                <div>
                  <h3 className="text-xs font-bold text-slate-900">Phase 2: Master Resume Ingestion</h3>
                  <p className="text-xs text-slate-600 mt-0.5">
                    PDF & DOCX parsing, entity extraction (skills, work history, education), and interactive editor.
                  </p>
                </div>
              </div>

              <div className="p-4 rounded-2xl bg-slate-50 border border-slate-200 flex items-start gap-3 opacity-75">
                <div className="w-5 h-5 rounded-full border-2 border-slate-300 flex items-center justify-center text-slate-400 text-[10px] font-bold mt-0.5">
                  3
                </div>
                <div>
                  <h3 className="text-xs font-bold text-slate-900">Phase 3: AI Resume Optimizer & ATS Match Scorer</h3>
                  <p className="text-xs text-slate-600 mt-0.5">
                    Semantic JD analysis, keyword gap highlighter, tailored resume generation with visual diff inspector.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Quick Profile Card */}
        <div className="space-y-6">
          <div className="p-6 rounded-3xl bg-white border border-slate-200/80 shadow-sm">
            <h2 className="text-base font-bold text-slate-900 mb-4">
              Verified Candidate Info
            </h2>
            <div className="space-y-3 text-xs">
              <div>
                <span className="text-slate-400 font-medium">Full Name</span>
                <p className="font-bold text-slate-800">{profile?.firstName} {profile?.lastName}</p>
              </div>
              <div>
                <span className="text-slate-400 font-medium">Primary Email</span>
                <p className="font-semibold text-slate-800">{user?.email}</p>
              </div>
              <div>
                <span className="text-slate-400 font-medium">Target Location</span>
                <p className="font-semibold text-slate-800 flex items-center gap-1 mt-0.5">
                  <MapPin className="w-3.5 h-3.5 text-slate-400" />
                  <span>{profile?.city}, {profile?.country}</span>
                </p>
              </div>
              <div>
                <span className="text-slate-400 font-medium">Billing Currency</span>
                <p className="font-semibold text-slate-800">{profile?.preferredCurrency}</p>
              </div>

              <div className="pt-2 border-t border-slate-100">
                <Link
                  to="/profile"
                  className="inline-flex items-center gap-1.5 text-xs font-bold text-brand-600 hover:text-brand-700"
                >
                  <span>Edit Profile Information</span>
                  <ArrowRight className="w-3.5 h-3.5" />
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
