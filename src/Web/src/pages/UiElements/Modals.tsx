import { PropsWithChildren, ReactNode } from "react";


interface ModalContent {
    header?:string;
    body?:ReactNode;
    handleClickWithEvent?:(e: React.MouseEvent<HTMLButtonElement>)=>void;
    isWide?:boolean
}



const Modals: React.FC<PropsWithChildren<ModalContent>> = ({
    header,
    body,
    handleClickWithEvent,
    isWide = false,
}) => {

    const modalClass = isWide
        ? "relative w-full max-w-5xl rounded-3xl bg-white dark:bg-gray-900 shadow-2xl p-6 lg:p-8"
        : "relative w-full max-w-xl rounded-3xl bg-white dark:bg-gray-900 shadow-2xl p-6 lg:p-8";

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-5">

            {/* Backdrop */}
            <div
                className="absolute inset-0 bg-black/40 backdrop-blur-sm"
                onClick={() => handleClickWithEvent}
            />

            {/* Modal */}
            <div
                className={modalClass}
                onClick={(e) => e.stopPropagation()}
            >

                {/* Close */}
                <button
                    onClick={handleClickWithEvent}
                    className="absolute top-4 right-4 flex h-10 w-10 items-center justify-center rounded-full bg-gray-100 hover:bg-gray-200 dark:bg-gray-800 dark:hover:bg-gray-700"
                >
                    ✕
                </button>

                {/* Header */}
                {header && (
                    <div className="border-b border-gray-200 dark:border-gray-700 pb-4 mb-6">
                        <h2 className="text-xl font-semibold text-gray-800 dark:text-white">
                            {header}
                        </h2>
                    </div>
                )}

                {/* Body */}
                <div className="max-h-[80vh] overflow-y-auto">
                    {body}
                </div>

            </div>

        </div>
    );
};
export default Modals;