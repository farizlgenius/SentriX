import { useMemo } from "react";
import { AuditTrailDto, EntityDiffResponse } from "../../model/Event/AuditTrailDto";

export const AuditDiffViewer: React.FC<AuditTrailDto> = ({ detail }) => {
  // 1. Safely parse the detail string or return the object
  const { parsedDiff, parseError } = useMemo(() => {
    if (typeof detail === 'object' && detail !== null) {
      return { parsedDiff: detail, parseError: null };
    }

    if (typeof detail === 'string') {
      if (!detail.trim()) {
        return { parsedDiff: null, parseError: null };
      }
      try {
        const parsed = JSON.parse(detail) as EntityDiffResponse;
        return { parsedDiff: parsed, parseError: null };
      } catch (err) {
        return {
          parsedDiff: null,
          parseError: 'Invalid JSON format in audit detail string.',
        };
      }
    }

    return { parsedDiff: null, parseError: null };
  }, [detail]);

  // Helper to format values cleanly (distinguishes empty strings from null/undefined)
  const formatValue = (val: any): string => {
    if (val === null || val === undefined) return 'null';
    if (val === '') return '""'; // Visual indicator for empty string
    if (typeof val === 'boolean') return val ? 'true' : 'false';
    if (typeof val === 'object') return JSON.stringify(val);
    return String(val);
  };

  // Badge styling based on entity status
  const getBadgeStyle = (state?: EntityDiffResponse['entityState']) => {
    switch (state) {
      case 'Added':
        return 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20';
      case 'Deleted':
        return 'bg-rose-500/10 text-rose-400 border-rose-500/20';
      case 'Modified':
        return 'bg-amber-500/10 text-amber-400 border-amber-500/20';
      default:
        return 'bg-slate-500/10 text-slate-400 border-slate-500/20';
    }
  };

  // Render error state if JSON parsing failed
  if (parseError) {
    return (
      <div className="w-full max-w-4xl rounded-lg border border-rose-900/50 bg-rose-950/20 p-4 font-mono text-xs text-rose-400">
        ⚠️ {parseError}
      </div>
    );
  }

  // Render empty state if no diff payload is provided
  if (!parsedDiff) {
    return (
      <div className="w-full max-w-4xl rounded-lg border border-slate-800 bg-slate-950 p-6 text-center font-mono text-sm text-slate-500">
        No audit log details available.
      </div>
    );
  }

  const { entityName, entityState, changes = {} } = parsedDiff;
  const hasChanges = Object.keys(changes).length > 0;

  return (
    <div className="w-full max-w-4xl rounded-lg border border-slate-800 bg-slate-950 font-mono text-sm text-slate-200 shadow-xl overflow-hidden">
      {/* Header Bar */}
      <div className="flex items-center justify-between border-b border-slate-800 bg-slate-900/80 px-4 py-3">
        <div className="flex items-center gap-3">
          <span className="font-semibold text-slate-100">{entityName || 'Unknown Entity'}</span>
          <span
            className={`rounded-full border px-2.5 py-0.5 text-xs font-medium ${getBadgeStyle(
              entityState
            )}`}
          >
            {entityState || 'Unchanged'}
          </span>
        </div>
        <span className="text-xs text-slate-500">
          {Object.keys(changes).length} field(s) changed
        </span>
      </div>

      {/* Diff Body */}
      {!hasChanges ? (
        <div className="p-6 text-center text-slate-500">
          No field changes detected.
        </div>
      ) : (
        <div className="divide-y divide-slate-800/60">
          {Object.entries(changes).map(([field, change]) => (
            <div key={field} className="group py-2">
              {/* Field Label Header */}
              <div className="px-4 py-1 text-xs font-semibold text-slate-400">
                {field}
              </div>

              {/* Git Red Line (Old / Removed Value) */}
              {entityState !== 'Added' && change.old !== null && change.old !== undefined && (
                <div className="flex items-center bg-rose-950/30 px-4 py-1 text-rose-300 transition-colors group-hover:bg-rose-950/50">
                  <span className="w-6 shrink-0 select-none font-bold text-rose-500">-</span>
                  <span className="line-through decoration-rose-500/50">
                    {formatValue(change.old)}
                  </span>
                </div>
              )}

              {/* Git Green Line (New / Added Value) */}
              {entityState !== 'Deleted' && change.new !== null && change.new !== undefined && (
                <div className="flex items-center bg-emerald-950/30 px-4 py-1 text-emerald-300 transition-colors group-hover:bg-emerald-950/50">
                  <span className="w-6 shrink-0 select-none font-bold text-emerald-500">+</span>
                  <span>{formatValue(change.new)}</span>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};