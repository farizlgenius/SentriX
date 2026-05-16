import { ModuleIcon } from "../../../icons";
import { Table, TableBody, TableCell, TableHeader, TableRow } from "../../ui/table";

export const AeroModuleDetailForm = () => {

      return (
            <>
                  <div className="flex flex-col gap-4 rounded-[20px]  bg-[var(--app-panel-bg)] p-5 lg:flex-row lg:items-center lg:justify-between">
                        <div>
                              <h3 className="text-lg font-semibold text-gray-900 dark:text-white">Modules Detail</h3>
                              <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-gray-400">Name the location, assign its country, and add a short description.</p>
                        </div>

                  </div>
                  {renderModuleCard()}
            </>
      )
}

const renderModuleCard = () => {

      return (
            <>
                  <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)]">
                        <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] ">
                              <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
                                    <TableRow >
                                          <TableCell className="text-center">Type</TableCell>
                                          <TableCell className="text-center">Name</TableCell>
                                          <TableCell className="text-center">Firmware</TableCell>
                                          <TableCell className="text-center">Module</TableCell>
                                          <TableCell className="text-center">Module</TableCell>
                                          <TableCell className="text-center">Module</TableCell>
                                    </TableRow>
                              </TableHeader>
                              <TableBody >
                                    <TableRow >
                                          <TableCell className="flex justify-center">
                                                <div className="flex h-14 w-14 items-center justify-center rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] text-gray-700 dark:text-gray-200">
                                                      <ModuleIcon className="text-2xl" />
                                                </div>
                                          </TableCell>
                                          <TableCell className="text-center">Test</TableCell>
                                    </TableRow>
                                    <TableRow >
                                          <TableCell className="flex justify-center">
                                                <div className="flex h-14 w-14 items-center justify-center rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] text-gray-700 dark:text-gray-200">
                                                      <ModuleIcon className="text-2xl" />
                                                </div>
                                          </TableCell>
                                          <TableCell className="text-center">Test</TableCell>

                                    </TableRow>
                              </TableBody>
                        </Table>
                  </div>

            </>

      );
};