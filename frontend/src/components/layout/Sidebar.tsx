import React from 'react';
import { NavLink } from 'react-router-dom';
import {
  LayoutDashboard,
  FileText,
  Search,
  KanbanSquare,
  Sparkles,
  User,
  ShieldCheck,
  CreditCard,
  Briefcase
} from 'lucide-react';

interface NavItem {
  name: string;
  to: string;
  icon: React.ElementType;
  badge?: string;
}

export const Sidebar: React.FC = () => {
  const mainNav: NavItem[] = [
    { name: 'Dashboard', to: '/dashboard', icon: LayoutDashboard },
    { name: 'Master Resume', to: '/resume', icon: FileText, badge: 'Phase 2' },
    { name: 'Job Discovery', to: '/jobs', icon: Search, badge: 'Phase 4' },
    { name: 'AI Analyzer', to: '/analyzer', icon: Sparkles, badge: 'Phase 3' },
    { name: 'Application Tracker', to: '/applications', icon: KanbanSquare, badge: 'Phase 5' },
    { name: 'Interview Prep', to: '/interview', icon: Briefcase, badge: 'Phase 7' },
  ];

  const secondaryNav: NavItem[] = [
    { name: 'My Profile', to: '/profile', icon: User },
    { name: 'Subscription', to: '/billing', icon: CreditCard },
  ];

  return (
    <aside className="w-64 bg-white border-r border-slate-200 flex flex-col justify-between p-4 min-h-[calc(100vh-4rem)]">
      <div className="space-y-6">
        <div>
          <div className="text-[11px] font-bold uppercase tracking-wider text-slate-400 px-3 mb-2">
            Application Suite
          </div>
          <nav className="space-y-1">
            {mainNav.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  `flex items-center justify-between px-3 py-2 rounded-xl text-sm font-semibold transition-all ${
                    isActive
                      ? 'bg-brand-50 text-brand-700 font-bold'
                      : 'text-slate-600 hover:text-slate-900 hover:bg-slate-50'
                  }`
                }
              >
                <div className="flex items-center gap-2.5">
                  <item.icon className="w-4 h-4" />
                  <span>{item.name}</span>
                </div>
                {item.badge && (
                  <span className="text-[10px] font-medium px-2 py-0.5 rounded-full bg-slate-100 text-slate-500">
                    {item.badge}
                  </span>
                )}
              </NavLink>
            ))}
          </nav>
        </div>

        <div>
          <div className="text-[11px] font-bold uppercase tracking-wider text-slate-400 px-3 mb-2">
            Account & Preferences
          </div>
          <nav className="space-y-1">
            {secondaryNav.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  `flex items-center gap-2.5 px-3 py-2 rounded-xl text-sm font-semibold transition-all ${
                    isActive
                      ? 'bg-brand-50 text-brand-700 font-bold'
                      : 'text-slate-600 hover:text-slate-900 hover:bg-slate-50'
                  }`
                }
              >
                <item.icon className="w-4 h-4" />
                <span>{item.name}</span>
              </NavLink>
            ))}
          </nav>
        </div>
      </div>

      <div className="p-3 bg-gradient-to-br from-brand-500/10 to-indigo-500/10 rounded-2xl border border-brand-200/50">
        <div className="flex items-center gap-2 text-brand-800 text-xs font-bold mb-1">
          <ShieldCheck className="w-4 h-4 text-brand-600" />
          <span>Assisted Apply Mode</span>
        </div>
        <p className="text-[11px] text-slate-600 leading-relaxed">
          Zero risk of portal bans. Master Resume invariant fact verification is active.
        </p>
      </div>
    </aside>
  );
};

