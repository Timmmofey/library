"use client"
import { useState } from 'react';
import { Button, Form, Input, InputNumber, Modal, notification } from 'antd';
import axios from 'axios';
import { API_BASE_URL } from '../../../const';
import Cookies from 'js-cookie';
import { useReaderAuthStore } from '@/stores/readerAuthStore';


const ExtendSubscriptionPage = () => {
  const [form] = Form.useForm();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [price, setPrice] = useState(0);
  const [loading, setLoading] = useState(false);
  const [hasSubmitted, setHasSubmitted] = useState(false);
  const updateReaderInfo = useReaderAuthStore((state) => state.fetchUserInfo);
  

  const handleCalculatePrice = (days) => {
    const costPerDay = 15;
    setPrice(days * costPerDay);
  };

  const handleOpenModal = () => {
    setHasSubmitted(true);
    form
      .validateFields()
      .then(() => {
        setIsModalVisible(true);
      })
      .catch(() => {
        // Errors will be shown for invalid fields
      });
  };

  const handleExtendSubscription = async () => {
    setLoading(true);
    try {
      const values = form.getFieldsValue();
      const response = await axios.post(`${API_BASE_URL}/api/Reader/extendSubscription?days=${values.days}`, {
        days: values.days,
        cardNumber: values.cardNumber,
        expiryDate: values.expiryDate,
        cvc: values.cvc,
      }, {
        headers: {
          Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
        }});
      notification.success({
        message: 'Успех',
        description: `Подписка успешно продлена. ID операции: ${response.data}`,
      });
      await updateReaderInfo();
      form.resetFields();
      setHasSubmitted(false);
    } catch (error) {
      if (error.response) {
        notification.error({
          message: 'Ошибка',
          description: error.response.data?.Message || 'Не удалось продлить подписку.',
        });
      } else {
        notification.error({
          message: 'Ошибка',
          description: 'Произошла неизвестная ошибка.',
        });
      }
    } finally {
      setLoading(false);
      setIsModalVisible(false);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '50px auto' }}>
      <h2>Продление подписки</h2>
      <Form
        form={form}
        layout="vertical"
        onValuesChange={(changedValues) => {
          if (changedValues.days) handleCalculatePrice(changedValues.days);
        }}
      >
        <Form.Item
          name="days"
          label="Количество дней"
          validateTrigger={hasSubmitted ? ['onChange', 'onBlur'] : 'onBlur'}
          rules={[{ required: true, message: 'Пожалуйста, введите количество дней' }]}
        >
          <InputNumber min={1} placeholder="Введите количество дней" style={{ width: '100%' }} />
        </Form.Item>

        <Form.Item
          name="cardNumber"
          label="Номер карты"
          validateTrigger={hasSubmitted ? ['onChange', 'onBlur'] : 'onBlur'}
          rules={[
            { required: true, message: 'Пожалуйста, введите номер карты' },
            {
              validator: (_, value) =>
                value && /^\d{4} \d{4} \d{4} \d{4}$/.test(value)
                  ? Promise.resolve()
                  : Promise.reject(new Error('Номер карты должен содержать 16 цифр')),
            },
          ]}
        >
          <Input
            placeholder="Введите номер карты"
            maxLength={19}
            onChange={(e) => {
              const formattedValue = e.target.value
                .replace(/\D/g, '')
                .replace(/(\d{4})(\d{1,4})/, '$1 $2')
                .replace(/(\d{4} \d{4})(\d{1,4})/, '$1 $2')
                .replace(/(\d{4} \d{4} \d{4})(\d{1,4})/, '$1 $2')
                .trim();
              form.setFieldsValue({ cardNumber: formattedValue });
            }}
          />
        </Form.Item>

        <Form.Item
          name="expiryDate"
          label="Срок действия карты"
          validateTrigger={hasSubmitted ? ['onChange', 'onBlur'] : 'onBlur'}
          rules={[
            { required: true, message: 'Пожалуйста, введите срок действия карты' },
            {
              validator: (_, value) =>
                value && /^(0[1-9]|1[0-2])\/(\d{2})$/.test(value)
                  ? Promise.resolve()
                  : Promise.reject(new Error('Введите срок в формате MM/YY')),
            },
          ]}
        >
          <Input
            placeholder="MM/YY"
            maxLength={5}
            onChange={(e) => {
              const formattedValue = e.target.value
                .replace(/\D/g, '')
                .replace(/(\d{2})(\d{1,2})/, '$1/$2')
                .slice(0, 5);
              form.setFieldsValue({ expiryDate: formattedValue });
            }}
          />
        </Form.Item>

        <Form.Item
          name="cvc"
          label="CVC код"
          validateTrigger={hasSubmitted ? ['onChange', 'onBlur'] : 'onBlur'}
          rules={[
            { required: true, message: 'Пожалуйста, введите CVC код' },
            {
              validator: (_, value) =>
                value && /^\d{3}$/.test(value)
                  ? Promise.resolve()
                  : Promise.reject(new Error('CVC код должен содержать 3 цифры')),
            },
          ]}
        >
          <Input
            placeholder="Введите CVC код"
            maxLength={3}
            onChange={(e) => {
              const formattedValue = e.target.value.replace(/\D/g, '').slice(0, 3);
              form.setFieldsValue({ cvc: formattedValue });
            }}
          />
        </Form.Item>

        <Form.Item>
          <Button type="primary" onClick={handleOpenModal} block>
            Рассчитать и подтвердить
          </Button>
        </Form.Item>
      </Form>

      <Modal
        title="Подтверждение продления"
        open={isModalVisible}
        onOk={handleExtendSubscription}
        onCancel={() => setIsModalVisible(false)}
        confirmLoading={loading}
      >
        <p>Вы собираетесь продлить подписку на <strong>{form.getFieldValue('days')}</strong> дней.</p>
        <p>Общая стоимость: <strong>{price} рублей</strong>.</p>
        <p>Номер карты: <strong>{form.getFieldValue('cardNumber')}</strong></p>
        <p>Вы уверены, что хотите продолжить?</p>
      </Modal>
    </div>
  );
};

export default ExtendSubscriptionPage;

// Важно: настройте прокси или измените URL-адрес для API вызова '/api/extendSubscription'.
