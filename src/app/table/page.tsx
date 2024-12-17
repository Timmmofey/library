// "use client";

// import { useEffect, useState } from 'react';
// import axios from 'axios';
// import clsx from 'clsx';

// interface LoanDto {
//     id: string;
//     issueDate: string;
//     dueDate: string;
//     returnDate?: string | null;
// }

// interface LibrarianDto {
//     id: string;
//     name: string;
//     readingRoomName?: string;
//     libraryName?: string;
//     role: string; // Поле role в строковом формате для отображения
//     loans?: LoanDto[];
// }

// const LibrariansPage: React.FC = () => {
//     const [librarians, setLibrarians] = useState<LibrarianDto[]>([]);
//     const [loading, setLoading] = useState<boolean>(true);
//     const [error, setError] = useState<string>('');

//     useEffect(() => {
//         const fetchLibrarians = async () => {
//             setLoading(true);
//             try {
//                 const response = await axios.get<LibrarianDto[]>('http://localhost:5251/librarians/search'); 
//                 setLibrarians(response.data);
//                 console.log(response.data);
//             } catch (err) {
//                 setError('Ошибка загрузки данных');
//             } finally {
//                 setLoading(false);
//             }
//         };

//         fetchLibrarians();
//     }, []);

//     if (loading) return <p className="text-center text-gray-500">Загрузка...</p>;
//     if (error) return <p className="text-center text-red-500">{error}</p>;
//     if (librarians.length === 0) return <p className="text-center text-gray-500">Библиотекари не найдены.</p>;

//     return (
//         <div className="container mx-auto p-4">
//             <h1 className="text-2xl font-bold mb-4">Список библиотекарей</h1>
//             <table className="min-w-full bg-white border border-gray-300 rounded-lg">
//                 <thead>
//                     <tr className="bg-gray-200">
//                         <th className="py-2 px-4 border-b">Имя</th>
//                         <th className="py-2 px-4 border-b">Название библиотеки</th>
//                         <th className="py-2 px-4 border-b">Читальный зал</th>
//                         <th className="py-2 px-4 border-b">Роль</th>
//                         <th className="py-2 px-4 border-b">Количество займов</th>
//                     </tr>
//                 </thead>
//                 <tbody>
//                     {librarians.map((librarian) => (
//                         <tr key={librarian.id} className={clsx(librarian.readingRoomName !== 'малый' ? "hover:bg-gray-100" : "bg-amber-400")}>
//                             <td className={clsx(librarian.readingRoomName !== 'малый' ? "py-2 px-4 border-b" : "py-2 px-4 border-b text-red-600")}>{librarian.name}</td>
//                             <td className={clsx(librarian.readingRoomName !== 'малый' ? "py-2 px-4 border-b" : "py-2 px-4 border-b text-red-600")}>{librarian.libraryName || 'Не указано'}</td>
//                             <td className={clsx(librarian.readingRoomName !== 'малый' ? "py-2 px-4 border-b" : "py-2 px-4 border-b text-red-600")}>{librarian.readingRoomName || 'Не указано'}</td>
//                             <td className="py-2 px-4 border-b">{librarian.role}</td>
//                             <td className="py-2 px-4 border-b">{librarian.loans ? librarian.loans.length : 0}</td>
//                         </tr>
//                     ))}
//                 </tbody>
//             </table>
//         </div>
//     );
// };

// export default LibrariansPage;
