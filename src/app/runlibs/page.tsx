"use client";

import React, { useState, useEffect } from "react";
import { Card, Select, Button, Input, Modal, Form, message } from "antd";
import axios from "axios";

import { API_BASE_URL } from "../../../const";

const { Option } = Select;

// Types
interface Librarian {
  id: string;
  name: string;
  login: string;
  libraryId: string;
  readingRoomId: string;
  role: number;
}

interface Library {
  id: string;
  name: string;
}

interface ReadingRoom {
  id: string;
  name: string;
  libraryId: string;
}

interface SearchParams {
  name?: string;
  libraryName?: string;
}

const LibrariansPage: React.FC = () => {
  const [librarians, setLibrarians] = useState<Librarian[]>([]);
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [readingRooms, setReadingRooms] = useState<ReadingRoom[]>([]);
  const [searchParams, setSearchParams] = useState<SearchParams>({});
  const [isAddModalOpen, setAddModalOpen] = useState(false);
  const [addLibrarianForm] = Form.useForm();
  const [isEditModalOpen, setEditModalOpen] = useState(false);
  const [selectedLibrarian, setSelectedLibrarian] = useState<Librarian | null>(
    null
  );
  const [editForm] = Form.useForm();
  const [filteredReadingRooms, setFilteredReadingRooms] = useState<ReadingRoom[]>(
    []
  );

  useEffect(() => {
    // Fetch libraries
    axios
      .get<Library[]>(`${API_BASE_URL}/Library`, { withCredentials: true })
      .then((res) => setLibraries(res.data))
      .catch(() => message.error("Failed to load libraries."));
  }, []);

  useEffect(() => {
    // Fetch reading rooms
    axios
      .get<ReadingRoom[]>(`${API_BASE_URL}/api/ReadingRooms`, {
        withCredentials: true,
      })
      .then((res) => setReadingRooms(res.data))
      .catch(() => message.error("Failed to load reading rooms."));
  }, []);

  useEffect(() => {
    if (selectedLibrarian) {
      // Filter reading rooms based on the selected librarian's libraryId
      const filteredRooms = readingRooms.filter(
        (room) => room.libraryId === selectedLibrarian.libraryId
      );
      setFilteredReadingRooms(filteredRooms);
    }
  }, [selectedLibrarian, readingRooms]);

  const handleSearch = () => {
    // Search librarians
    axios
      .get<Librarian[]>(`${API_BASE_URL}/Librarians/search?role=2`, {
        params: searchParams,
        withCredentials: true,
      })
      .then((res) => setLibrarians(res.data))
      .catch(() => message.error("Librarians not found."));
  };

  const handleAddLibrarian = (values: {
    name: string;
    login: string;
    passwordHash: string;
    libraryId: string;
    readingRoomId: string;
  }) => {
    axios
      .post(
        `${API_BASE_URL}/Librarians/addLibrarian`,
        { ...values, role: 2 },
        { withCredentials: true }
      )
      .then(() => {
        message.success("Librarian successfully added!");
        setAddModalOpen(false);
        addLibrarianForm.resetFields();
        handleSearch();
      })
      .catch(() => message.error("Failed to add librarian."));
  };

  const handleEditClick = (id: string) => {
    axios
      .get<Librarian>(`${API_BASE_URL}/Librarians/${id}`, {
        withCredentials: true,
      })
      .then((res) => {
        setSelectedLibrarian(res.data);
        setEditModalOpen(true);
        editForm.setFieldsValue(res.data);
      })
      .catch(() => message.error("Failed to load librarian data."));
  };

  const handleEditSubmit = (values: Partial<Librarian>) => {
    if (!selectedLibrarian) return;

    axios
      .put(
        `${API_BASE_URL}/Librarians/${selectedLibrarian.id}`,
        { ...values, role: 2 },
        { withCredentials: true }
      )
      .then(() => {
        message.success("Librarian updated successfully!");
        setEditModalOpen(false);
        handleSearch();
      })
      .catch(() => message.error("Failed to update librarian."));
  };

  const handleDelete = () => {
    if (!selectedLibrarian) return;

    Modal.confirm({
      title: "Are you sure you want to delete this librarian?",
      content: `Deleting librarian "${selectedLibrarian.name}" will result in loss of related data.`,
      okText: "Delete",
      okType: "danger",
      cancelText: "Cancel",
      onOk: () => {
        axios
          .delete(`${API_BASE_URL}/Librarians/${selectedLibrarian.id}`, {
            withCredentials: true,
          })
          .then(() => {
            message.success("Librarian successfully deleted!");
            setEditModalOpen(false);
            handleSearch();
          })
          .catch(() => message.error("Failed to delete librarian."));
      },
    });
  };

  return (
    <div style={{ padding: "20px" }}>
      <h1 style={{ fontSize: "30px" }}>Librarians</h1>
      <Button
        type="primary"
        style={{ marginBottom: "20px" }}
        onClick={() => setAddModalOpen(true)}
      >
        Add Librarian
      </Button>

      {/* Search Form */}
      <h2>Search Form</h2>
      <div style={{ marginBottom: "20px" }}>
        <Input
          placeholder="Enter librarian name"
          onChange={(e) =>
            setSearchParams({ ...searchParams, name: e.target.value })
          }
          style={{ width: 200, marginRight: 10 }}
        />
        <Select
          placeholder="Select library"
          onChange={(value) =>
            setSearchParams({ ...searchParams, libraryName: value })
          }
          style={{ width: 200, marginRight: 10 }}
          allowClear
        >
          {libraries.map((library) => (
            <Option key={library.id} value={library.name}>
              {library.name}
            </Option>
          ))}
        </Select>
        <Button type="primary" onClick={handleSearch}>
          Search
        </Button>
      </div>

      {/* Librarians List */}
      <div style={{ display: "flex", flexWrap: "wrap", gap: "20px" }}>
        {librarians.map((librarian) => (
          <Card
            key={librarian.id}
            title={librarian.name}
            style={{ width: 300 }}
            extra={
              <Button type="link" onClick={() => handleEditClick(librarian.id)}>
                Edit
              </Button>
            }
          >
            <p>
              <strong>Login:</strong> {librarian.login}
            </p>
            <p>
              <strong>Library:</strong>{" "}
              {libraries.find((lib) => lib.id === librarian.libraryId)?.name ||
                "Unknown"}
            </p>
            <p>
              <strong>Reading Room:</strong>{" "}
              {
                readingRooms.find(
                  (room) => room.id === librarian.readingRoomId
                )?.name
              }
            </p>
          </Card>
        ))}
      </div>

      {/* Add Librarian Modal */}
      <Modal
        title="Add Librarian"
        open={isAddModalOpen}
        onCancel={() => setAddModalOpen(false)}
        footer={null}
      >
        <Form form={addLibrarianForm} layout="vertical" onFinish={handleAddLibrarian}>
          <Form.Item
            name="name"
            label="Name"
            rules={[{ required: true, message: "Enter name" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="login"
            label="Login"
            rules={[{ required: true, message: "Enter login" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="passwordHash"
            label="Password"
            rules={[{ required: true, message: "Enter password" }]}
          >
            <Input.Password />
          </Form.Item>
          <Form.Item
            name="libraryId"
            label="Library"
            rules={[{ required: true, message: "Select library" }]}
          >
            <Select>
              {libraries.map((library) => (
                <Option key={library.id} value={library.id}>
                  {library.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item
            name="readingRoomId"
            label="Reading Room"
            rules={[{ required: true, message: "Select reading room" }]}
          >
            <Select>
              {readingRooms.map((room) => (
                <Option key={room.id} value={room.id}>
                  {room.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Button type="primary" htmlType="submit">
            Add
          </Button>
        </Form>
      </Modal>

      {/* Edit Librarian Modal */}
      <Modal
        title="Edit Librarian"
        open={isEditModalOpen}
        onCancel={() => setEditModalOpen(false)}
        footer={null}
      >
        <Form form={editForm} layout="vertical" onFinish={handleEditSubmit}>
          <Form.Item name="name" label="Name">
            <Input />
          </Form.Item>
          <Form.Item name="login" label="Login">
            <Input />
          </Form.Item>
          {/* No Library field */}
          <Form.Item name="readingRoomId" label="Reading Room">
            <Select>
              {filteredReadingRooms.map((room) => (
                <Option key={room.id} value={room.id}>
                  {room.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Button type="primary" htmlType="submit">
            Update
          </Button>
          <Button type="danger" onClick={handleDelete} style={{ marginLeft: 10 }}>
            Delete
          </Button>
        </Form>
      </Modal>
    </div>
  );
};

export default LibrariansPage;
